using System;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using Microsoft.HockeyApp;
using MoneyFox.Business.Manager;
using MoneyFox.Windows.Views;
using MoneyFox.Foundation.Constants;

namespace MoneyFox.Windows
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
        private void SetColor()
        {
            // We have to create a own local settings object here since the general dependency 
            // registration takes place later and the Theme can only be set in the constructor.
            RequestedTheme = new WindowsUwpSettings().GetValue(SettingsManager.DARK_THEME_SELECTED_KEYNAME, false)
                ? ApplicationTheme.Dark
                : ApplicationTheme.Light;
        }

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var shell = Window.Current.Content as AppShell;

            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                var extendedSplash = new ExtendedSplashScreen(e.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }

            OverrideTitleBarColor();
            
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void OverrideTitleBarColor()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // set up our brushes
            var bkgColor = Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
            var backgroundColor = Current.Resources["AppBarBrush"] as SolidColorBrush;
            var appForegroundColor = Current.Resources["AppForegroundPrimaryBrush"] as SolidColorBrush;

            // override colors!
            if (bkgColor != null && appForegroundColor != null)
            {
                // If on a mobile device set the status bar
                if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                {
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