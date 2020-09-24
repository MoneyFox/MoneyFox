using Autofac;
using MoneyFox.Application.Common;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.Ui.Shared.ViewModels.Backup;
using MoneyFox.Ui.Shared.ViewModels.Settings;
using MoneyFox.Uwp.Activation;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.Payments;
using MoneyFox.Uwp.ViewModels.Settings;
using MoneyFox.Uwp.ViewModels.Statistic;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;
using MoneyFox.Uwp.Views;
using MoneyFox.Uwp.Views.Accounts;
using MoneyFox.Uwp.Views.Payments;
using MoneyFox.Uwp.Views.Settings;
using MoneyFox.Uwp.Views.Statistics;
using MoneyFox.Uwp.Views.Statistics.StatisticCategorySummary;
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
            if(ConfigurationManager.AppSettings != null) return;

            InitConfig();
            ExecutingPlatform.Current = AppPlatform.UWP;
            ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["WindowsAppcenterSecret"], typeof(Analytics), typeof(Crashes));
            Analytics.TrackEvent("AppStarted");
#endif

            LoggerService.Initialize();

            ConfigureNavigation();
            RegisterServices();

            await JumpListService.InitializeAsync();
            ThemeSelectorService.Initialize();
        }

        private void InitConfig()
        {
            ConfigurationManager.Initialise(PortableStream.Current);
            if(ConfigurationManager.AppSettings == null)
                throw new ConfigFailedToInitException();
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
            NavigationService.Register<AddPaymentViewModel, AddPaymentView>();
            NavigationService.Register<EditPaymentViewModel, EditPaymentView>();
            NavigationService.Register<CategoryListViewModel, CategoryListView>();
            NavigationService.Register<SettingsViewModel, SettingsView>();
            NavigationService.Register<StatisticCashFlowViewModel, StatisticCashFlowView>();
            NavigationService.Register<StatisticCategorySpreadingViewModel, StatisticCategorySpreadingView>();
            NavigationService.Register<StatisticCategorySummaryViewModel, StatisticCategorySummaryView>();
            NavigationService.Register<StatisticSelectorViewModel, StatisticSelectorView>();
            NavigationService.Register<BackupViewModel, BackupView>();
            NavigationService.Register<WindowsSettingsViewModel, SettingsView>();
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            if(IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(defaultNavItem);
                if(defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }
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
