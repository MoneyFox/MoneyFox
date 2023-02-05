namespace MoneyFox.Ui.InversionOfControl;

using Common.Services;
using Core.Common.Interfaces;
using Core.Interfaces;
using Core.InversionOfControl;
using Infrastructure.Adapters;
using Mapping;
using MoneyFox.Infrastructure.InversionOfControl;
using Views.About;
using Views.Accounts;
using Views.Accounts.AccountModification;
using Views.Backup;
using Views.Budget;
using Views.Budget.BudgetModification;
using Views.Categories;
using Views.Categories.CategorySelection;
using Views.Categories.ModifyCategory;
using Views.Dashboard;
using Views.OverflowMenu;
using Views.Payments;
using Views.Payments.PaymentModification;
using Views.Popups;
using Views.Settings;
using Views.Setup;
using Views.Setup.CurrencyIntroduction;
using Views.Statistics.CashFlow;
using Views.Statistics.CategoryProgression;
using Views.Statistics.CategorySpreading;
using Views.Statistics.CategorySummary;
using Views.Statistics.MonthlyAccountCashFlow;
using Views.Statistics.Selector;

public sealed class MoneyFoxConfig
{
    public void Register(IServiceCollection serviceCollection)
    {
        RegisterServices(serviceCollection);
        RegisterSetupViewModels(serviceCollection);
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
    private static void RegisterSetupViewModels(IServiceCollection services)
    {
        _ = services.AddTransient<WelcomeViewModel>()
            .AddTransient<SetupCurrencyViewModel>()
            .AddTransient<SetupAccountsViewModel>()
            .AddTransient<SetupCategoryViewModel>()
            .AddTransient<SetupCompletionViewModel>();
    }

    private static void RegisterViewModels(IServiceCollection services)
    {
        _ = services.AddTransient<AboutViewModel>()
            .AddTransient<AccountListViewModel>()
            .AddTransient<AddAccountViewModel>()
            .AddTransient<EditAccountViewModel>()
            .AddTransient<AddCategoryViewModel>()
            .AddTransient<CategoryListViewModel>()
            .AddTransient<EditCategoryViewModel>()
            .AddTransient<CategorySelectionViewModel>()
            .AddTransient<DashboardViewModel>()
            .AddTransient<BackupViewModel>()
            .AddTransient<OverflowMenuViewModel>()
            .AddTransient<AddPaymentViewModel>()
            .AddTransient<EditPaymentViewModel>()
            .AddTransient<PaymentListViewModel>()
            .AddTransient<SettingsViewModel>()
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
