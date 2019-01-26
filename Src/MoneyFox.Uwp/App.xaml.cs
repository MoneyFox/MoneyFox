using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyFox.DataLayer;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Uwp.Views;
using MvvmCross;
using MvvmCross.Platforms.Uap.Views;
using PCLAppConfig;
using UniversalRateReminder;

#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif


namespace MoneyFox.Uwp
{

	public abstract class MoneyFoxApp : MvxApplication<Setup, CoreApp>
	{
	}

	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App
	{
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
		}

		private void SetTheme()
		{
			//switch (new SettingsManager(new SettingsAdapter()).Theme)
			//{
			//	case AppTheme.Dark:
			//		RequestedTheme = ApplicationTheme.Dark;
			//		break;

			//	case AppTheme.Light:
			//		RequestedTheme = ApplicationTheme.Light;
			//		break;
			//}
		}

		private MainView mainView;

		/// <summary>
		///     Invoked when the application is launched normally by the end user.  Other entry points
		///     will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override async void OnLaunched(LaunchActivatedEventArgs e)
		{
			CoreApp.CurrentPlatform = AppPlatform.UWP;
			base.OnLaunched(e);

			ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);

#if !DEBUG
			AppCenter.Start("1fba816a-eea6-42a8-bf46-0c0fcc1589db", typeof(Analytics), typeof(Crashes));
#endif

			if (e.PreviousExecutionState != ApplicationExecutionState.Running)
			{

				ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

				Xamarin.Forms.Forms.Init(e);
				new Presentation.App();

				//BackgroundTaskHelper.Register(typeof(ClearPaymentsTask), new TimeTrigger(60, false));
				//BackgroundTaskHelper.Register(typeof(RecurringPaymentTask), new TimeTrigger(60, false));
				//BackgroundTaskHelper.Register(typeof(LiveTiles), new TimeTrigger(15, false));

                mainView.ViewModel = Mvx.IoCProvider.Resolve<MainViewModel>();

                //if (!Mvx.IoCProvider.CanResolve<Session>()) return;

                //if (Mvx.IoCProvider.Resolve<Session>().ValidateSession())
                //{
                //    (mainView.ViewModel as MainViewModel)?.ShowAccountListCommand.ExecuteAsync();
                //}
                //else if (Mvx.IoCProvider.CanResolve<IMvxNavigationService>())
                //{
                //    await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<LoginViewModel>();
                //}

                (mainView.ViewModel as MainViewModel)?.ShowAccountListCommand.ExecuteAsync();

                OverrideTitleBarColor();

				//If Jump Lists are supported, add them
				if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList"))
				{
					await SetJumplist();
				}

				await CallRateReminder();
			}
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

            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Disabled;
        }

		private async Task SetJumplist()
		{
			var jumpList = await JumpList.LoadCurrentAsync();
			jumpList.Items.Clear();
			jumpList.SystemGroupKind = JumpListSystemGroupKind.None;

			var listItemAddIncome = JumpListItem.CreateWithArguments(Constants.ADD_INCOME_TILE_ID,
																	 Strings.AddIncomeLabel);
			listItemAddIncome.Logo = new Uri("ms-appx:///Assets/IncomeTileIcon.png");
			jumpList.Items.Add(listItemAddIncome);

			var listItemAddSpending = JumpListItem.CreateWithArguments(Constants.ADD_EXPENSE_TILE_ID,
																	   Strings.AddExpenseLabel);
			listItemAddSpending.Logo = new Uri("ms-appx:///Assets/SpendingTileIcon.png");
			jumpList.Items.Add(listItemAddSpending);

			var listItemAddTransfer = JumpListItem.CreateWithArguments(Constants.ADD_TRANSFER_TILE_ID,
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

			//new SettingsManager(new SettingsAdapter()).SessionTimestamp = DateTime.Now.AddMinutes(-15).ToString(CultureInfo.CurrentCulture);

			deferral.Complete();
		}
	}
}