using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Uwp.BackgroundTasks;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.Views;
using NLog;
using UnhandledExceptionEventArgs = Windows.UI.Xaml.UnhandledExceptionEventArgs;
using Windows.ApplicationModel.Background;

namespace MoneyFox.Uwp
{
    public sealed partial class App
    {
        private readonly Lazy<ActivationService> activationService;

        private ActivationService ActivationService => activationService.Value;

        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;
            UnhandledException += OnUnhandledException;

            activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated) await ActivationService.ActivateAsync(args);

            OverrideTitleBarColor();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(AccountListViewModel), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new AppShell();
        }

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);

            IBackgroundTaskInstance taskInstance = args.TaskInstance;

            new ClearPaymentsTask().Run(taskInstance);
            new RecurringPaymentTask().Run(taskInstance);
            await new SyncBackupTask().RunAsync(taskInstance);
        }

        private static void OverrideTitleBarColor()
        {
            //draw into the title bar
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            //remove the solid-colored backgrounds behind the caption controls and system back button
            ApplicationViewTitleBar viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonForegroundColor = Colors.LightGray;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogManager.GetCurrentClassLogger().Fatal(e.Exception);
        }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        public static void OnSuspending(object sender, SuspendingEventArgs e)
        {
            LogManager.GetCurrentClassLogger().Info("Application Suspending.");
            LogManager.Shutdown();
        }
    }
}
