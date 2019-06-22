using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using Microsoft.Toolkit.Uwp.Helpers;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.DataLayer;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation;
using MoneyFox.ServiceLayer.Facades;
using PCLAppConfig;
using UniversalRateReminder;
using MoneyFox.Uwp.Tasks;
using GenericServices;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Uwp.Views;
using NLog;
using NLog.Targets;
using LogLevel = NLog.LogLevel;
using NavigationService = GalaSoft.MvvmLight.Views.NavigationService;

#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif


namespace MoneyFox.Uwp
{
    /// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App
    {
        private Logger logManager;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
		{
			InitializeComponent();
			SetTheme();

            EfCoreContext.DbPath = DatabaseConstants.DB_NAME;
            Suspending += OnSuspending;
            UnhandledException += OnUnhandledException;
        }

        private void SetTheme()
        {
            switch (new SettingsFacade(new SettingsAdapter()).Theme)
            {
                case AppTheme.Dark:
                    RequestedTheme = ApplicationTheme.Dark;
                    break;

                case AppTheme.Light:
                    RequestedTheme = ApplicationTheme.Light;
                    break;
            }
        }

		/// <summary>
		///     Invoked when the application is launched normally by the end user.  Other entry points
		///     will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="activationArgs">Details about the launch request and process.</param>
		protected override async void OnLaunched(LaunchActivatedEventArgs activationArgs)
        {
            InitLogger();

            logManager.Info("Application Started.");
            logManager.Info("App Version: {Version}", new WindowsAppInformation().GetVersion());
            
			base.OnLaunched(activationArgs);
            var mainView = new AppShell();

            if (activationArgs.PreviousExecutionState != ApplicationExecutionState.Running)
			{
			    ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
                ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

#if !DEBUG
                AppCenter.Start(ConfigurationManager.AppSettings["WindowsAppcenterSecret"], typeof(Analytics), typeof(Crashes));
#endif

                RegisterServices(mainView.MainFrame);

                Xamarin.Forms.Forms.Init(activationArgs);
				//var app = new Presentation.App();

                BackgroundTaskHelper.Register(typeof(ClearPaymentsTask), new TimeTrigger(60, false));
                BackgroundTaskHelper.Register(typeof(RecurringPaymentTask), new TimeTrigger(60, false));
                BackgroundTaskHelper.Register(typeof(LiveTiles), new TimeTrigger(15, false));
                
                OverrideTitleBarColor();

				//If Jump Lists are supported, add them
				if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList"))
				{
					await SetJumplist();
				}

				await CallRateReminder();
            }

            if (Window.Current.Content == null)
            {
                Window.Current.Content = mainView;
            }
            Window.Current.Activate();

            if (activationArgs.TileActivatedInfo != null)
            {
                await HandleTileActivationInfo(activationArgs);
            }
        }

        private void RegisterServices(Frame mainFrame)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(ConfigureNavigation(mainFrame)).AsImplementedInterfaces();

            builder.RegisterModule<WindowsModule>();
            ViewModelLocator.RegisterServices(builder);
        }

        public NavigationService ConfigureNavigation(Frame mainFrame)
        {
            var nav = new NavigationService {CurrentFrame = mainFrame};

            nav.Configure(ViewModelLocator.AccountList, typeof(AccountListView));
            nav.Configure(ViewModelLocator.PaymentList, typeof(PaymentListView));
            nav.Configure(ViewModelLocator.CategoryList, typeof(CategoryListView));
            nav.Configure(ViewModelLocator.SelectCategoryList, typeof(SelectCategoryListView));
            nav.Configure(ViewModelLocator.AddAccount, typeof(AddAccountView));
            nav.Configure(ViewModelLocator.AddCategory, typeof(AddCategoryView));
            nav.Configure(ViewModelLocator.AddPayment, typeof(AddPaymentView));
            nav.Configure(ViewModelLocator.EditAccount, typeof(EditAccountView));
            nav.Configure(ViewModelLocator.EditCategory, typeof(EditCategoryView));
            nav.Configure(ViewModelLocator.EditPayment, typeof(EditPaymentView));
            nav.Configure(ViewModelLocator.Settings, typeof(SettingsView));
            nav.Configure(ViewModelLocator.StatisticSelector, typeof(StatisticSelectorView));
            nav.Configure(ViewModelLocator.StatisticCashFlow, typeof(StatisticCashFlowView));
            nav.Configure(ViewModelLocator.StatisticCategorySpreading, typeof(StatisticCategorySpreadingView));
            nav.Configure(ViewModelLocator.StatisticCategorySummary, typeof(StatisticCategorySummaryView));

            return nav;
        }

        private async Task HandleTileActivationInfo(LaunchActivatedEventArgs activationArgs)
        {
            logManager.Debug("Passed TileID: {tileId}", activationArgs.TileId);

            if (int.TryParse(activationArgs.TileId, out int accountId))
            {
                logManager.Info("Open Payment List of Account with ID {accountId}", accountId);
                
                var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
                var crudServices = ServiceLocator.Current.GetInstance<ICrudServicesAsync>();
                AccountViewModel acct = await crudServices.ReadSingleAsync<AccountViewModel>(accountId);

                navigationService.NavigateTo(ViewModelLocator.PaymentList, acct.Id);
            }
        }

        private void InitLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new FileTarget("logfile")
            {
                FileName = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "moneyfox.log"),
                AutoFlush = true,
                ArchiveEvery = FileArchivePeriod.Month
            };
            var debugTarget = new DebugTarget("console");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
            logManager = LogManager.GetCurrentClassLogger();
        }

  //      protected override Frame InitializeFrame(IActivatedEventArgs activationArgs)
		//{
		//	mainView = new MainView { Language = ApplicationLanguages.Languages[0] };
		//	Window.Current.Content = mainView;
		//	mainView.MainFrame.NavigationFailed += OnNavigationFailed;

		//	RootFrame = mainView.MainFrame;
		//	return RootFrame;
		//}

		private void OverrideTitleBarColor()
		{
			//draw into the title bar
			CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            //remove the solid-colored backgrounds behind the caption controls and system back button
            ApplicationViewTitleBar viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
			viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
			viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
			viewTitleBar.ButtonForegroundColor = Colors.LightGray;
        }

		private async Task SetJumplist()
		{
			var jumpList = await JumpList.LoadCurrentAsync();
			jumpList.Items.Clear();
			jumpList.SystemGroupKind = JumpListSystemGroupKind.None;

			var listItemAddIncome = JumpListItem.CreateWithArguments(AppConstants.ADD_INCOME_TILE_ID,
																	 Strings.AddIncomeLabel);
			listItemAddIncome.Logo = new Uri("ms-appx:///Assets/IncomeTileIcon.png");
			jumpList.Items.Add(listItemAddIncome);

			var listItemAddSpending = JumpListItem.CreateWithArguments(AppConstants.ADD_EXPENSE_TILE_ID,
																	   Strings.AddExpenseLabel);
			listItemAddSpending.Logo = new Uri("ms-appx:///Assets/SpendingTileIcon.png");
			jumpList.Items.Add(listItemAddSpending);

			var listItemAddTransfer = JumpListItem.CreateWithArguments(AppConstants.ADD_TRANSFER_TILE_ID,
																	   Strings.AddTransferLabel);
			listItemAddTransfer.Logo = new Uri("ms-appx:///Assets/TransferTileIcon.png");
			jumpList.Items.Add(listItemAddTransfer);

			await jumpList.SaveAsync();
		}

		private async Task CallRateReminder()
		{
			RatePopup.RateButtonText = Strings.YesLabel;
			RatePopup.CancelButtonText = Strings.NotNowLabel;
			RatePopup.Title = Strings.RateReminderTitle;
			RatePopup.Content = Strings.RateReminderText;

			await RatePopup.CheckRateReminderAsync();
        }

        private void OnUnhandledException(object sender, global::Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            logManager.Fatal(e.Exception);
        }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        public void OnSuspending(object sender, SuspendingEventArgs e)
        {
            logManager.Info("Application Suspending.");
            LogManager.Shutdown();
        }
    }
}