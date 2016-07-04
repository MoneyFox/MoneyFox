using System;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Cheesebaron.MvxPlugins.Settings.WindowsCommon;
using Microsoft.HockeyApp;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MoneyFox.Windows.Services;
using MoneyFox.Windows.Shortcuts;
using MoneyFox.Windows.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using UniversalRateReminder;

namespace MoneyFox.Windows {
    /// <summary>
    ///     Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App {
        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            InitializeComponent();
            SetColor();
            Suspending += OnSuspending;
#if !DEBUG
            HockeyClient.Current.Configure(ServiceConstants.HOCKEY_APP_WINDOWS_ID,
                new TelemetryConfiguration {EnableDiagnostics = true});
#endif
        }

        /// <summary>
        ///     Bind the saved theme from settings to the root element which cascadingly applies to children elements
        ///     the reason this is bound in code behind is that because viewmodels are loaded after the pages,
        ///     resulting to a nullreference exception if bound in xaml.
        /// </summary>
        private void SetColor() {
            // We have to create a own local settings object here since the general dependency 
            // registration takes place later and the Theme can only be set in the constructor.
            RequestedTheme = new WindowsCommonSettings().GetValue(SettingsHelper.DARK_THEME_SELECTED, false)
                ? ApplicationTheme.Dark
                : ApplicationTheme.Light;
        }

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e) {
            var shell = Window.Current.Content as AppShell;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (shell == null) {
                //TODO: check if this can be removed with the neutral language set.
                // Create a AppShell to act as the navigation context and navigate to the first page
                shell = new AppShell {Language = ApplicationLanguages.Languages[0]};

                shell.MyAppFrame.NavigationFailed += OnNavigationFailed;
            }

            // Place our app shell in the current Window
            Window.Current.Content = shell;
            ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

            if (shell.MyAppFrame.Content == null) {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                var setup = new Setup(shell.MyAppFrame);
                setup.Initialize();

                var start = Mvx.Resolve<IMvxAppStart>();
                start.Start();

                shell.ViewModel = Mvx.Resolve<MenuViewModel>();
            }

            new TileHelper().DoNavigation(string.IsNullOrEmpty(e.Arguments)
                ? e.TileId
                : e.Arguments);

            await new BackgroundTaskService().RegisterTasksAsync();

            OverrideTitleBarColor();

            //If Jump Lists are supported, adds them
            if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList")) {
                SetJumplist();
            }

            CallRateReminder();

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async void CallRateReminder() {
            RatePopup.RateButtonText = Strings.YesLabel;
            RatePopup.CancelButtonText = Strings.NotNowLabel;
            RatePopup.Title = Strings.RateReminderTitle;
            RatePopup.Content = Strings.RateReminderText;

            await RatePopup.CheckRateReminderAsync();
        }

        private void OverrideTitleBarColor() {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // set up our brushes
            var bkgColor = Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
            var backgroundColor = Current.Resources["AppBarBrush"] as SolidColorBrush;
            var appForegroundColor = Current.Resources["AppForegroundPrimaryBrush"] as SolidColorBrush;

            // override colors!
            if (bkgColor != null && appForegroundColor != null) {
                // If on a mobile device set the status bar
                if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar")) {
                    StatusBar.GetForCurrentView().BackgroundColor = backgroundColor?.Color;
                    StatusBar.GetForCurrentView().BackgroundOpacity = 0.6;
                    StatusBar.GetForCurrentView().ForegroundColor = appForegroundColor.Color;
                }

                titleBar.BackgroundColor = backgroundColor?.Color;
                titleBar.ButtonBackgroundColor = backgroundColor?.Color;

                titleBar.ForegroundColor = Colors.White;
                titleBar.ButtonForegroundColor = Colors.White;
            }
        }

        private async void SetJumplist() {
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

        /// <summary>
        ///     Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            Tile.UpdateMainTile();

            SettingsHelper.SessionTimestamp = DateTime.Now.AddMinutes(-15).ToString(CultureInfo.CurrentCulture);

            deferral.Complete();
        }
    }
}