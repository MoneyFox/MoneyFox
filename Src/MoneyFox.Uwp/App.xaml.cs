using System;
using System.Globalization;
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
using Microsoft.Toolkit.Uwp.Helpers;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.DataLayer;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation;
using MoneyFox.ServiceLayer.Authentication;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Uwp.Views;
using MvvmCross;
using MvvmCross.Platforms.Uap.Views;
using PCLAppConfig;
using UniversalRateReminder;
using MoneyFox.Uwp.Tasks;
using MvvmCross.Navigation;
using GenericServices;
using MoneyFox.ServiceLayer.Parameters;
using NLog;
using NLog.Targets;
using LogLevel = NLog.LogLevel;
#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif


namespace MoneyFox.Uwp
{

	public abstract class MoneyFoxApp : MvxApplication<ApplicationSetup, CoreApp>
	{
	}

	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App : IDisposable
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
			Suspending += OnSuspending;

			EfCoreContext.DbPath = DatabaseConstants.DB_NAME;
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

		private MainView mainView;

		/// <summary>
		///     Invoked when the application is launched normally by the end user.  Other entry points
		///     will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="activationArgs">Details about the launch request and process.</param>
		protected override async void OnLaunched(LaunchActivatedEventArgs activationArgs)
        {
            InitLogger();

            logManager.Info("Application Start.");

            CoreApp.CurrentPlatform = AppPlatform.UWP;
			base.OnLaunched(activationArgs);

            if (activationArgs.PreviousExecutionState != ApplicationExecutionState.Running)
			{
			    ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
                ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];
#if !DEBUG
                AppCenter.Start(ConfigurationManager.AppSettings["WindowsAppcenterSecret"], typeof(Analytics), typeof(Crashes));
#endif

                Xamarin.Forms.Forms.Init(activationArgs);
				var app = new Presentation.App();

                BackgroundTaskHelper.Register(typeof(ClearPaymentsTask), new TimeTrigger(60, false));
                BackgroundTaskHelper.Register(typeof(RecurringPaymentTask), new TimeTrigger(60, false));
                BackgroundTaskHelper.Register(typeof(LiveTiles), new TimeTrigger(15, false));

                mainView.ViewModel = Mvx.IoCProvider.Resolve<MainViewModel>();

                if (!Mvx.IoCProvider.CanResolve<Session>()) return;

                if (Mvx.IoCProvider.Resolve<Session>().ValidateSession())
                {
                    (mainView.ViewModel as MainViewModel)?.ShowAccountListCommand.ExecuteAsync();
                } 
                else if (Mvx.IoCProvider.CanResolve<IMvxNavigationService>()) {
                    await Mvx.IoCProvider
                             .Resolve<IMvxNavigationService>()
                             .Navigate<LoginViewModel>()                             ;
                }

                ((MainViewModel) mainView.ViewModel)?.ShowAccountListCommand.ExecuteAsync();

                OverrideTitleBarColor();

				//If Jump Lists are supported, add them
				if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList"))
				{
					await SetJumplist();
				}

				await CallRateReminder();
			}

            if (activationArgs.TileActivatedInfo != null)
            {
                await HandleTileActivationInfo(activationArgs);
            }
        }

        private async Task HandleTileActivationInfo(LaunchActivatedEventArgs activationArgs)
        {
            logManager.Debug("Passed TileID: {tileId}", activationArgs.TileId);
            if (Mvx.IoCProvider.CanResolve<IMvxNavigationService>()
                && Mvx.IoCProvider.CanResolve<ICrudServicesAsync>()
                && int.TryParse(activationArgs.TileId, out int accountId))
            {
                logManager.Info("Open Payment List of Account with ID {accountId}", accountId);
                
                var navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
                var crudServices = Mvx.IoCProvider.Resolve<ICrudServicesAsync>();
                AccountViewModel acct = await crudServices.ReadSingleAsync<AccountViewModel>(accountId);
                await navigationService.Navigate<PaymentListViewModel, PaymentListParameter>(new PaymentListParameter(acct.Id));
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

        protected override Frame InitializeFrame(IActivatedEventArgs activationArgs)
		{
			mainView = new MainView { Language = ApplicationLanguages.Languages[0] };
			Window.Current.Content = mainView;
			mainView.MainFrame.NavigationFailed += OnNavigationFailed;

			RootFrame = mainView.MainFrame;
			return RootFrame;
		}

		protected override Frame CreateFrame()
		{
			return mainView.MainFrame;
		}

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
        private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
            LogManager.Shutdown();
            new SettingsFacade(new SettingsAdapter()).SessionTimestamp = DateTime.Now.AddMinutes(-15).ToString(CultureInfo.CurrentCulture);

			deferral.Complete();
		}

	    public void Dispose()
	    {
	        mainView?.Dispose();
	    }
	}
}