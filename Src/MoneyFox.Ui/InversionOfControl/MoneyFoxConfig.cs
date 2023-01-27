namespace MoneyFox.Ui.InversionOfControl;

using Common.Services;
using Core.Common.Interfaces;
using Core.Interfaces;
using Core.InversionOfControl;
using Infrastructure.Adapters;
using Mapping;
using MoneyFox.Infrastructure.InversionOfControl;
using ViewModels.Payments;
using ViewModels.Statistics;
using Views.About;
using Views.Accounts;
using Views.Backup;
using Views.Budget;
using Views.Categories;
using Views.Categories.ModifyCategory;
using Views.Dashboard;
using Views.OverflowMenu;
using Views.Popups;
using Views.Settings;
using Views.SetupAssistant;
using Views.Statistics.CategorySummary;
using Views.Statistics.Selector;

public sealed class MoneyFoxConfig
{
    public void Register(IServiceCollection serviceCollection)
    {
        RegisterServices(serviceCollection);
        RegisterViewModels(serviceCollection);
        RegisterAdapters(serviceCollection);
        _ = serviceCollection.AddSingleton(_ => AutoMapperFactory.Create());
        new CoreConfig().Register(serviceCollection);
        InfrastructureConfig.Register(serviceCollection);
    }

    private static void RegisterServices(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddSingleton<IDialogService, DialogService>()
            .AddTransient<INavigationService, NavigationService>()
            .AddTransient<IToastService, ToastService>();
    }

    private static void RegisterViewModels(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<AboutViewModel>()
            .AddTransient<AccountListViewModel>()
            .AddTransient<AddAccountViewModel>()
            .AddTransient<EditAccountViewModel>()
            .AddTransient<AddCategoryViewModel>()
            .AddTransient<CategoryListViewModel>()
            .AddTransient<EditCategoryViewModel>()
            .AddTransient<SelectCategoryViewModel>()
            .AddTransient<DashboardViewModel>()
            .AddTransient<BackupViewModel>()
            .AddTransient<OverflowMenuViewModel>()
            .AddTransient<AddPaymentViewModel>()
            .AddTransient<EditPaymentViewModel>()
            .AddTransient<PaymentListViewModel>()
            .AddTransient<SettingsViewModel>()
            .AddTransient<CategoryIntroductionViewModel>()
            .AddTransient<SetupCompletionViewModel>()
            .AddTransient<WelcomeViewModel>()
            .AddTransient<PaymentForCategoryListViewModel>()
            .AddTransient<StatisticAccountMonthlyCashFlowViewModel>()
            .AddTransient<StatisticCashFlowViewModel>()
            .AddTransient<StatisticCategoryProgressionViewModel>()
            .AddTransient<StatisticCategorySpreadingViewModel>()
            .AddTransient<StatisticCategorySummaryViewModel>()
            .AddTransient<StatisticSelectorViewModel>()
            .AddTransient<SelectDateRangeDialogViewModel>()
            .AddTransient<SelectFilterDialogViewModel>()
            .AddTransient<AddBudgetViewModel>()
            .AddTransient<EditBudgetViewModel>()
            .AddTransient<BudgetListViewModel>();
    }

    private static void RegisterAdapters(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<IBrowserAdapter, BrowserAdapter>()
            .AddTransient<IConnectivityAdapter, ConnectivityAdapter>()
            .AddTransient<IEmailAdapter, EmailAdapter>()
            .AddTransient<ISettingsAdapter, SettingsAdapter>();
    }
}
