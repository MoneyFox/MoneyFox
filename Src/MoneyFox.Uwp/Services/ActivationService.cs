using Autofac;
using MoneyFox.Application.Common;
using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.Ui.Shared.ViewModels.Backup;
using MoneyFox.Ui.Shared.ViewModels.Settings;
using MoneyFox.Uwp.Activation;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.Statistic;
using MoneyFox.Uwp.Views;
using MoneyFox.Uwp.Views.Settings;
using MoneyFox.Uwp.Views.Statistics;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Xaml;
using Frame = Windows.UI.Xaml.Controls.Frame;

#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace MoneyFox.Uwp.Services
{
    internal class ActivationService
    {
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
                await InitializeAsync(activationArgs);

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

        private async Task InitializeAsync(object activationArgs)
        {
            ExecutingPlatform.Current = AppPlatform.UWP;
            ConfigurationManager.Initialise(PortableStream.Current);
            ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["WindowsAppcenterSecret"], typeof(Analytics), typeof(Crashes));
#endif

            LoggerService.Initialize();

            ConfigureNavigation();
            RegisterServices();

            await JumpListService.InitializeAsync();
            ThemeSelectorService.Initialize();
        }

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<WindowsModule>();
            ViewModelLocator.RegisterServices(builder);
        }

        private void ConfigureNavigation()
        {
            NavigationService.Register<AccountListViewModel, AccountListView>();
            NavigationService.Register<PaymentListViewModel, PaymentListView>();
            NavigationService.Register<CategoryListViewModel, CategoryListView>();
            NavigationService.Register<SettingsViewModel, SettingsView>();
            NavigationService.Register<StatisticCashFlowViewModel, StatisticCashFlowView>();
            NavigationService.Register<StatisticCategorySpreadingViewModel, StatisticCategorySpreadingView>();
            NavigationService.Register<StatisticCategorySummaryViewModel, StatisticCategorySummaryView>();
            NavigationService.Register<StatisticSelectorViewModel, StatisticSelectorView>();
            NavigationService.Register<BackupViewModel, BackupView>();
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            if(IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(defaultNavItem);
                if(defaultHandler.CanHandle(activationArgs))
                    await defaultHandler.HandleAsync(activationArgs);
            }
        }

        private static async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
            await RateDisplayService.ShowIfAppropriateAsync();
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }
    }
}
