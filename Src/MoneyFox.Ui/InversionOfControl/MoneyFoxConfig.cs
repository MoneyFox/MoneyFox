namespace MoneyFox.Ui.InversionOfControl;

using Common.Services;
using Controls.CategorySelection;
using Core.Common.Interfaces;
using Core.Interfaces;
using Core.InversionOfControl;
using Infrastructure.Adapters;
using Mapping;
using MoneyFox.Infrastructure.InversionOfControl;
using Navigation;
using Navigation.Impl;
using Views.About;
using Views.Accounts.AccountList;
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
using Views.Payments.PaymentList;
using Views.Payments.PaymentModification;
using Views.Settings;
using Views.Setup;
using Views.Setup.SelectCurrency;
using Views.Statistics;
using Views.Statistics.CashFlow;
using Views.Statistics.CategoryProgression;
using Views.Statistics.CategorySpreading;
using Views.Statistics.CategorySummary;
using Views.Statistics.MonthlyAccountCashFlow;
using Views.Statistics.Selector;
using INavigationService = Ui.INavigationService;

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
            .AddTransient<IViewLocator, ViewLocator>()
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
            .AddTransient<SelectCategoryViewModel>()
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
            .AddTransient<BudgetListViewModel>()
            .AddTransient<CategorySelectionViewModel>();
    }

    private static void RegisterAdapters(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<IBrowserAdapter, BrowserAdapter>()
            .AddTransient<IConnectivityAdapter, ConnectivityAdapter>()
            .AddTransient<IEmailAdapter, EmailAdapter>()
            .AddTransient<ISettingsAdapter, SettingsAdapter>();
    }
}
