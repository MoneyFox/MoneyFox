using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Xaml;
using Autofac;
using Microsoft.Toolkit.Helpers;
using MoneyFox.Application.Common;
using MoneyFox.Presentation;
using MoneyFox.Uwp.Activation;
using MoneyFox.Uwp.Views;
using MoneyFox.Uwp.Views.Settings;
using MoneyFox.Uwp.Views.Statistics;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using Xamarin.Forms;
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
        private readonly App app;
        private readonly Type defaultNavItem;
        private readonly Lazy<UIElement> shell;

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell)
        {
            this.app = app;
            this.shell = shell;
            this.defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize things like registering background task before the app is loaded
                await InitializeAsync(activationArgs);

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = shell?.Value ?? new Frame();
                }
            }

            await HandleActivationAsync(activationArgs);

            if (IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }

        private async Task InitializeAsync(object activationArgs)
        {
            ExecutingPlatform.Current = AppPlatform.UWP;
            ConfigurationManager.Initialise(PortableStream.Current);
            ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

#if !DEBUG
            //AppCenter.Start(ConfigurationManager.AppSettings["WindowsAppcenterSecret"], typeof(Analytics), typeof(Crashes));
#endif

            LoggerService.Initialize();

            NavigationService navService = ConfigureNavigation();
            RegisterServices(navService);

            Forms.Init(activationArgs as LaunchActivatedEventArgs);
            new Presentation.App();

            await Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasksAsync();
            await JumpListService.InitializeAsync();
            ThemeSelectorService.Initialize(app.RequestedTheme);
        }

        private static void RegisterServices(NavigationService nav)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(nav)
                   .AsImplementedInterfaces()
                   .AsSelf();

            builder.RegisterModule<WindowsModule>();
            ViewModelLocator.RegisterServices(builder);
        }

        public NavigationService ConfigureNavigation()
        {
            var nav = new NavigationService();

            nav.Configure(ViewModelLocator.AccountList, typeof(AccountListView));
            nav.Configure(ViewModelLocator.PaymentList, typeof(PaymentListView));
            nav.Configure(ViewModelLocator.CategoryList, typeof(CategoryListView));
            nav.Configure(ViewModelLocator.SelectCategoryList, typeof(SelectCategoryListView));
            nav.Configure(ViewModelLocator.AddAccount, typeof(AddAccountView));
            nav.Configure(ViewModelLocator.AddCategory, typeof(AddCategoryView));
            nav.Configure(ViewModelLocator.AddPayment, typeof(AddPaymentView));
            nav.Configure(ViewModelLocator.EditAccount, typeof(EditAccountView));
            nav.Configure(ViewModelLocator.EditCategory, typeof(EditCategoryView));
            nav.Configure(ViewModelLocator.EditPayment, typeof(EditPaymentView));
            nav.Configure(ViewModelLocator.Settings, typeof(SettingsView));
            nav.Configure(ViewModelLocator.StatisticCashFlow, typeof(StatisticCashFlowView));
            nav.Configure(ViewModelLocator.StatisticCategorySpreading, typeof(StatisticCategorySpreadingView));
            nav.Configure(ViewModelLocator.StatisticCategorySummary, typeof(StatisticCategorySummaryView));
            nav.Configure(ViewModelLocator.Backup, typeof(BackupView));
            nav.Configure(ViewModelLocator.About, typeof(AboutView));

            return nav;
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = GetActivationHandlers().FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs)) await defaultHandler.HandleAsync(activationArgs);
            }
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<BackgroundTaskService>.Instance;
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
