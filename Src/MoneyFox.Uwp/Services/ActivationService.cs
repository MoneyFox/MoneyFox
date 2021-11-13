using Autofac;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Uwp.Activation;
using MoneyFox.Uwp.ViewModels.Payments;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

#nullable enable
namespace MoneyFox.Uwp.Services
{
    internal class ActivationService
    {
        private const float CHART_LABEL_SIZE = 12f;

        private readonly Type defaultNavItem;
        private readonly Lazy<UIElement> shell;

        public ActivationService(Type defaultNavItem, Lazy<UIElement> shell)
        {
            this.shell = shell;
            this.defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if(IsInteractive(activationArgs))
            {
                // Initialize things like registering background task before the app is loaded
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                Window.Current.Content ??= shell?.Value ?? new Frame();
            }

            await HandleActivationAsync(activationArgs);

            if(IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }

            await StartupTasksService.StartupAsync();
        }

        private async Task InitializeAsync()
        {
            if(ConfigurationManager.AppSettings != null)
            {
                return;
            }

            InitConfig();
            ExecutingPlatform.Current = AppPlatform.UWP;
            ChartOptions.LabelTextSize = CHART_LABEL_SIZE;
            ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["WindowsAppcenterSecret"], typeof(Analytics), typeof(Crashes));
            Analytics.TrackEvent("AppStarted");
#endif

            LoggerService.Initialize();

            RegisterServices();

            NavigationServiceInitializerService.Initialize();
            await JumpListService.InitializeAsync();
            ThemeSelectorService.Initialize();
        }

        private void InitConfig()
        {
            ConfigurationManager.Initialise(PortableStream.Current);
            if(ConfigurationManager.AppSettings == null)
            {
                throw new FailedToInitConfiFileException();
            }
        }

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<WindowsModule>();
            ViewModelLocator.RegisterServices(builder);
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            if(IsInteractive(activationArgs))
            {
                string arguments = (activationArgs as LaunchActivatedEventArgs)?.Arguments ?? "";
                var defaultHandler = new DefaultLaunchActivationHandler(GetStartupView(arguments, defaultNavItem));
                if(defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }
            }
        }

        private static Type GetStartupView(string startupArgs, Type defaultView) =>
            startupArgs == AppConstants.AddPaymentId
                ? typeof(AddPaymentViewModel)
                : defaultView;

        private static async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
            await RateDisplayService.ShowIfAppropriateAsync();
        }

        private static bool IsInteractive(object args) => args is IActivatedEventArgs;
    }
}