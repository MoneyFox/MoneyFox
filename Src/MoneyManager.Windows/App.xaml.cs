using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Shortcut;
using MoneyManager.Windows.Views;

namespace MoneyManager.Windows
{
    /// <summary>
    ///     Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
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
        protected override void OnLaunched(LaunchActivatedEventArgs e)
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

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                var setup = new Setup(shell.AppFrame);
                setup.Initialize();

                var start = Mvx.Resolve<IMvxAppStart>();
                start.Start();
            }

            shell.AppFrame.Navigate(typeof (MainView));
            new TileHelper(Mvx.Resolve<ModifyTransactionViewModel>()).DoNavigation(e.TileId);

            Tile.UpdateMainTile();

            OverrideTitleBarColor();

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void OverrideTitleBarColor()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // set up our brushes
            var bkgColor = Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
            var btnHoverColor = Current.Resources["TitleBarButtonHoverThemeBrush"] as SolidColorBrush;
            var btnPressedColor = Current.Resources["TitleBarButtonPressedThemeBrush"] as SolidColorBrush;
            var backgroundColor = Current.Resources["TitleBarBackgroundThemeBrush"] as SolidColorBrush;
            var appForegroundColor = Current.Resources["AppForegroundBrush"] as SolidColorBrush;

            // override colors!
            if (bkgColor != null && btnHoverColor != null && btnPressedColor != null && appForegroundColor != null)
            {
                titleBar.BackgroundColor = bkgColor.Color;
                titleBar.ForegroundColor = appForegroundColor.Color;
                titleBar.ButtonBackgroundColor = bkgColor.Color;
                titleBar.ButtonForegroundColor = appForegroundColor.Color;
                titleBar.ButtonHoverBackgroundColor = btnHoverColor.Color;
                titleBar.ButtonHoverForegroundColor = appForegroundColor.Color;
                titleBar.ButtonPressedBackgroundColor = btnPressedColor.Color;
                titleBar.ButtonPressedForegroundColor = appForegroundColor.Color;
                titleBar.InactiveBackgroundColor = bkgColor.Color;
                titleBar.InactiveForegroundColor = appForegroundColor.Color;
                titleBar.ButtonInactiveBackgroundColor = bkgColor.Color;

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

            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: SaveItem application state and stop any background activity
            deferral.Complete();
        }
    }
}