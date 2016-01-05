using System;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Authentication;
using MoneyManager.Core.Helpers;
using MoneyManager.Core.ViewModels;
using MoneyManager.Localization;
using MoneyManager.Windows.Concrete;
using MoneyManager.Windows.Concrete.Services;
using MoneyManager.Windows.Concrete.Shortcut;
using MoneyManager.Windows.Views;
using UniversalRateReminder;
using Windows.UI.StartScreen;
using MoneyManager.Foundation;
using Windows.Foundation;

namespace MoneyManager.Windows
{
    /// <summary>
    ///     Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            var shell = Window.Current.Content as AppShell;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (shell == null)
            {
                // Create a AppShell to act as the navigation context and navigate to the first page
                shell = new AppShell {Language = ApplicationLanguages.Languages[0]};

                shell.AppFrame.NavigationFailed += OnNavigationFailed;
            }

            // Place our app shell in the current Window
            Window.Current.Content = shell;
            ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                var setup = new Setup(shell.AppFrame);
                setup.Initialize();

                var start = Mvx.Resolve<IMvxAppStart>();
                start.Start();
            }

            if (Mvx.Resolve<Session>().ValidateSession())
            {
                shell.SetLoggedInView();
                shell.AppFrame.Navigate(typeof (MainView));
            }
            else
            {
                shell.SetLoginView();
                shell.AppFrame.Navigate(typeof (LoginView));
            }

            new TileHelper(Mvx.Resolve<ModifyTransactionViewModel>()).DoNavigation(e.TileId);

            Tile.UpdateMainTile();
            await new BackgroundTaskService().RegisterTasksAsync();

            OverrideTitleBarColor();

            SetJumplist();

            CallRateReminder();

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async void CallRateReminder()
        {
            RatePopup.RateButtonText = Strings.YesLabel;
            RatePopup.CancelButtonText = Strings.NotNowLabel;
            RatePopup.Title = Strings.RateReminderTitle;
            RatePopup.Content = Strings.RateReminderText;

            await RatePopup.CheckRateReminderAsync();
        }

        private void OverrideTitleBarColor()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // set up our brushes
            var bkgColor = Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
            var backgroundColor = Current.Resources["TitleBarBackgroundThemeBrush"] as SolidColorBrush;
            var appForegroundColor = Current.Resources["AppForegroundBrush"] as SolidColorBrush;

            // override colors!
            if (bkgColor != null && appForegroundColor != null)
            {
                // If on a mobile device set the status bar
                if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                {
                    StatusBar.GetForCurrentView().BackgroundColor = backgroundColor?.Color;
                    StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                    StatusBar.GetForCurrentView().ForegroundColor = appForegroundColor.Color;
                }
                titleBar.ButtonInactiveForegroundColor = appForegroundColor.Color;
            }
        }

        private async void SetJumplist()
        {
            var jump_list = await JumpList.LoadCurrentAsync();
            jump_list.Items.Clear();
            jump_list.SystemGroupKind = JumpListSystemGroupKind.None; //The group for static links
            #region Create and Add the jump list (repeat for every jump list you want to add)
            JumpListItem list_item = JumpListItem.CreateWithArguments(Constants.INCOME_TILE_ID, Strings.AddIncomeLabel);
            list_item.Logo = new Uri("ms-appx:///Assets/IncomeTileIcon.png");
            jump_list.Items.Add(list_item);

            JumpListItem list_item2 = JumpListItem.CreateWithArguments(Constants.SPENDING_TILE_ID, Strings.AddSpendingLabel);
            list_item2.Logo = new Uri("ms-appx:///Assets/SpendingTileIcon.png");
            jump_list.Items.Add(list_item2);

            JumpListItem list_item3 = JumpListItem.CreateWithArguments(Constants.TRANSFER_TILE_ID, Strings.AddTransferLabel);
            list_item3.Logo = new Uri("ms-appx:///Assets/TransferTileIcon.png");
            jump_list.Items.Add(list_item3);

            #endregion
            await jump_list.SaveAsync();
        }


        /// <summary>
        ///     Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
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
            Tile.UpdateMainTile();

            Settings.SessionTimestamp = DateTime.Now.AddMinutes(-15).ToString(CultureInfo.CurrentCulture);

            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: SaveItem application state and stop any background activity
            deferral.Complete();
        }
    }
}