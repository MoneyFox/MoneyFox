using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Accounts;
using MoneyFox.Uwp.Views;
using NLog;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using UnhandledExceptionEventArgs = Windows.UI.Xaml.UnhandledExceptionEventArgs;

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
            if(!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }

            OverrideTitleBarColor();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService() =>
            new ActivationService(typeof(AccountListViewModel), new Lazy<UIElement>(CreateShell));

        private UIElement CreateShell() => new AppShell();

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args) =>
            await ActivationService.ActivateAsync(args);

        private static void OverrideTitleBarColor()
        {
            //draw into the title bar
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) =>
            LogManager.GetCurrentClassLogger().Fatal(e.Exception);

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved     without knowing
        ///     whether the application will be terminated or resumed with the contents     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            LogManager.GetCurrentClassLogger().Info("Application Suspending.");
            LogManager.Shutdown();
        }
    }
}