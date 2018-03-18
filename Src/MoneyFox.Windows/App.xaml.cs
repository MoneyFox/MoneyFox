using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using Microsoft.Toolkit.Uwp.Helpers;
using MoneyFox.Business.Converter;
#if !DEBUG
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
#endif
using MoneyFox.Business.Manager;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Windows.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using UniversalRateReminder;
using MoneyFox.Foundation.Resources;
using MoneyFox.Windows.Tasks;

namespace MoneyFox.Windows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
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

            ApplicationContext.DbPath = DatabaseConstants.DB_NAME;
        }

        private void SetTheme()
        {
            switch (new WindowsUwpSettings().GetValue(SettingsManager.THEME_KEYNAME, AppTheme.System))
            {
                case AppTheme.Dark:
                    RequestedTheme = ApplicationTheme.Dark;
                    break;

                case AppTheme.Light:
                    RequestedTheme = ApplicationTheme.Light;
                    break;

                case AppTheme.System:
                    var uiSettings = new UISettings();
                    var color = uiSettings.GetColorValue(UIColorType.Background);

                    RequestedTheme = color == Color.FromArgb(255, 255, 255, 255)
                        ? ApplicationTheme.Light
                        : ApplicationTheme.Dark;
                    break;
            }
        }

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if !DEBUG
            MobileCenter.Start("1fba816a-eea6-42a8-bf46-0c0fcc1589db", typeof(Analytics), typeof(Crashes));
#endif
            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                var mainView = new MainView { Language = ApplicationLanguages.Languages[0] };

                Window.Current.Content = mainView;
                ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                var setup = new Setup(mainView.MainFrame);
                setup.Initialize();

                var start = Mvx.Resolve<IMvxAppStart>();
                start.Start();

                Xamarin.Forms.Forms.Init(e);

                BackgroundTaskHelper.Register(typeof(ClearPaymentsTask), new TimeTrigger(60, false));
                BackgroundTaskHelper.Register(typeof(RecurringPaymentTask), new TimeTrigger(60, false));
                Mvx.Resolve<IBackgroundTaskManager>().StartBackupSyncTask(60);

                mainView.ViewModel = Mvx.Resolve<MenuViewModel>();

                OverrideTitleBarColor();

                //If Jump Lists are supported, add them
                if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList"))
                {
                    await SetJumplist();
                }

                await CallRateReminder();
            }

            //OverrideTitleBarColor();

            //When jumplist is selected navigate to appropriate tile
            var tileHelper = Mvx.Resolve<TileHelper>();
            if (e.Arguments == Constants.ADD_INCOME_TILE_ID)
            {
                await tileHelper.DoNavigation(Constants.ADD_INCOME_TILE_ID);
            }
            else if (e.Arguments == Constants.ADD_EXPENSE_TILE_ID)
            {
                await tileHelper.DoNavigation(Constants.ADD_EXPENSE_TILE_ID);
            }
            else if (e.Arguments == Constants.ADD_TRANSFER_TILE_ID)
            {
                await tileHelper.DoNavigation(Constants.ADD_TRANSFER_TILE_ID);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void OverrideTitleBarColor()
        {
            //draw into the title bar
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            //remove the solid-colored backgrounds behind the caption controls and system back button
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
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

        //private void OverrideTitleBarColor()
        //{
        //    var titleBar = ApplicationView.GetForCurrentView().TitleBar;

        //    // set up our brushes
        //    var bkgColor = Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
        //    var backgroundColor = Current.Resources["AppBarBrush"] as SolidColorBrush;
        //    var appForegroundColor = Current.Resources["AppForegroundPrimaryBrush"] as SolidColorBrush;

        //    // override colors!
        //    if (bkgColor != null && appForegroundColor != null)
        //    {
        //        // If on a mobile device set the status bar
        //        if (StatusBar.i)
        //        {
        //            StatusBar.GetForCurrentView().BackgroundColor = backgroundColor?.Color;
        //            StatusBar.GetForCurrentView().BackgroundOpacity = 0.6;
        //            StatusBar.GetForCurrentView().ForegroundColor = appForegroundColor.Color;
        //        }

        //        titleBar.BackgroundColor = backgroundColor?.Color;
        //        titleBar.ButtonBackgroundColor = backgroundColor?.Color;

        //        titleBar.ForegroundColor = Colors.White;
        //        titleBar.ButtonForegroundColor = Colors.White;
        //    }
        //}

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

            new SettingsManager(new WindowsUwpSettings()).SessionTimestamp = DateTime.Now.AddMinutes(-15).ToString(CultureInfo.CurrentCulture);

            deferral.Complete();
        }
    }
}