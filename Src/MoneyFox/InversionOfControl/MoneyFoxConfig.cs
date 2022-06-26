namespace MoneyFox.InversionOfControl
{

    using Common.Services;
    using Core.Common.Interfaces;
    using Core.Interfaces;
    using Core.InversionOfControl;
    using Mapping;
    using Microsoft.Extensions.DependencyInjection;
    using Mobile.Infrastructure.InversionOfControl;
    using ViewModels.About;
    using ViewModels.Accounts;
    using ViewModels.Budget;
    using ViewModels.Categories;
    using ViewModels.Dashboard;
    using ViewModels.DataBackup;
    using ViewModels.Dialogs;
    using ViewModels.OverflowMenu;
    using ViewModels.Payments;
    using ViewModels.Settings;
    using ViewModels.SetupAssistant;
    using ViewModels.Statistics;

    public sealed class MoneyFoxConfig
    {
        public void Register(ServiceCollection serviceCollection)
        {
            RegisterServices(serviceCollection);
            RegisterViewModels(serviceCollection);
            serviceCollection.AddSingleton(_ => AutoMapperFactory.Create());

            new CoreConfig().Register(serviceCollection);
            new InfrastructureMobileConfig().Register(serviceCollection);
        }

        private static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDialogService, DialogService>();
            serviceCollection.AddTransient<INavigationService, NavigationService>();
            serviceCollection.AddTransient<IToastService, ToastService>();
        }

        private static void RegisterViewModels(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AboutViewModel>();
            serviceCollection.AddTransient<AccountListViewModel>();
            serviceCollection.AddTransient<AddAccountViewModel>();
            serviceCollection.AddTransient<EditAccountViewModel>();
            serviceCollection.AddTransient<AddCategoryViewModel>();
            serviceCollection.AddTransient<CategoryListViewModel>();
            serviceCollection.AddTransient<EditCategoryViewModel>();
            serviceCollection.AddTransient<SelectCategoryViewModel>();
            serviceCollection.AddTransient<DashboardViewModel>();
            serviceCollection.AddTransient<BackupViewModel>();
            serviceCollection.AddTransient<OverflowMenuViewModel>();
            serviceCollection.AddTransient<AddPaymentViewModel>();
            serviceCollection.AddTransient<EditPaymentViewModel>();
            serviceCollection.AddTransient<PaymentListViewModel>();
            serviceCollection.AddTransient<SettingsViewModel>();
            serviceCollection.AddTransient<CategoryIntroductionViewModel>();
            serviceCollection.AddTransient<SetupCompletionViewModel>();
            serviceCollection.AddTransient<WelcomeViewModel>();
            serviceCollection.AddTransient<PaymentForCategoryListViewModel>();
            serviceCollection.AddTransient<StatisticAccountMonthlyCashFlowViewModel>();
            serviceCollection.AddTransient<StatisticCashFlowViewModel>();
            serviceCollection.AddTransient<StatisticCategoryProgressionViewModel>();
            serviceCollection.AddTransient<StatisticCategorySpreadingViewModel>();
            serviceCollection.AddTransient<StatisticCategorySummaryViewModel>();
            serviceCollection.AddTransient<StatisticSelectorViewModel>();

            serviceCollection.AddTransient<SelectDateRangeDialogViewModel>();
            serviceCollection.AddTransient<SelectFilterDialogViewModel>();

            serviceCollection.AddTransient<AddBudgetViewModel>();
            serviceCollection.AddTransient<EditBudgetViewModel>();
            serviceCollection.AddTransient<BudgetListPageViewModel>(); er
        }
    }

}
