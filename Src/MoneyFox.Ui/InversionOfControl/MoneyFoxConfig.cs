namespace MoneyFox.Ui.InversionOfControl;

using Common.Navigation;
using Common.Services;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Controls.CategorySelection;
using Core.Common.Interfaces;
using Core.Interfaces;
using Core.InversionOfControl;
using Infrastructure.Adapters;
using MoneyFox.Infrastructure.InversionOfControl;
using Views.About;
using Views.Accounts.AccountList;
using Views.Accounts.AccountModification;
using Views.Backup;
using Views.Budget;
using Views.Budget.BudgetModification;
using Views.Budget.BudgetOverview;
using Views.Categories;
using Views.Categories.CategorySelection;
using Views.Categories.ModifyCategory;
using Views.Dashboard;
using Views.OverflowMenu;
using Views.Payments.PaymentList;
using Views.Payments.PaymentModification;
using Views.Settings;
using Views.Setup;
using Views.Setup.SelectCurrency;
using Views.Setup.SetupAccounts;
using Views.Statistics;
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
        serviceCollection.AddSingleton<IViewLocator>(sp => new ViewLocator(sp));
        RegisterServices(serviceCollection);
        RegisterSetup(serviceCollection);
        RegisterPopups(serviceCollection);
        RegisterViewModels(serviceCollection);
        RegisterViews(serviceCollection);
        RegisterAdapters(serviceCollection);
        serviceCollection.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        new CoreConfig().Register(serviceCollection);
        InfrastructureConfig.Register(serviceCollection);
    }

    private static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDialogService, DialogService>();
        serviceCollection.AddTransient<IToastService, ToastService>();
        serviceCollection.AddTransient<INavigationService, NavigationService>();
    }

    private static void RegisterSetup(IServiceCollection services)
    {
        services.AddTransient<WelcomePage>();
        services.AddTransient<SetupCurrencyPage>();
        services.AddTransient<SetupAccountPage>();
        services.AddTransient<SetupCategoryPage>();
        services.AddTransient<SetupCompletionPage>();
        services.AddTransient<WelcomeViewModel>();
        services.AddTransient<SetupCurrencyViewModel>();
        services.AddTransient<SetupAccountsViewModel>();
        services.AddTransient<SetupCategoryViewModel>();
        services.AddTransient<SetupCompletionViewModel>();
        services.AddTransient<BudgetOverviewPage>();
    }

    private static void RegisterPopups(IServiceCollection services)
    {
        services.AddTransientPopup<FilterPopup, SelectFilterPopupViewModel>();
        services.AddTransientPopup<DateSelectionPopup, SelectDateRangeDialogViewModel>();
    }

    private static void RegisterViewModels(IServiceCollection services)
    {
        services.AddTransient<MainPageViewModel>();
        services.AddTransient<AboutViewModel>();
        services.AddTransient<AccountListViewModel>();
        services.AddTransient<AddAccountViewModel>();
        services.AddTransient<EditAccountViewModel>();
        services.AddTransient<AddCategoryViewModel>();
        services.AddTransient<CategoryListViewModel>();
        services.AddTransient<EditCategoryViewModel>();
        services.AddTransient<SelectCategoryViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<BackupViewModel>();
        services.AddTransient<OverflowMenuViewModel>();
        services.AddTransient<AddPaymentViewModel>();
        services.AddTransient<EditPaymentViewModel>();
        services.AddTransient<PaymentListViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<PaymentForCategoryListViewModel>();
        services.AddTransient<StatisticAccountMonthlyCashFlowViewModel>();
        services.AddTransient<StatisticCashFlowViewModel>();
        services.AddTransient<StatisticCategoryProgressionViewModel>();
        services.AddTransient<StatisticCategorySpreadingViewModel>();
        services.AddTransient<StatisticCategorySummaryViewModel>();
        services.AddTransient<StatisticSelectorViewModel>();
        services.AddTransient<SelectDateRangeDialogViewModel>();
        services.AddTransient<SelectFilterPopupViewModel>();
        services.AddTransient<AddBudgetViewModel>();
        services.AddTransient<EditBudgetViewModel>();
        services.AddTransient<BudgetListViewModel>();
        services.AddTransient<CategorySelectionViewModel>();
        services.AddTransient<BudgetOverviewViewModel>();
    }

    private static void RegisterViews(IServiceCollection services)
    {
        services.AddTransient<AddPaymentPage>();
        services.AddTransient<AccountListPage>();
        services.AddTransient<AddAccountPage>();
        services.AddTransient<EditAccountPage>();
        services.AddTransient<AddCategoryPage>();
        services.AddTransient<CategoryListPage>();
        services.AddTransient<EditCategoryPage>();
        services.AddTransient<SelectCategoryPage>();
        services.AddTransient<BackupPage>();
        services.AddTransient<AddPaymentPage>();
        services.AddTransient<EditPaymentPage>();
        services.AddTransient<PaymentListPage>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<AboutPage>();
        services.AddTransient<PaymentForCategoryListPage>();
        services.AddTransient<StatisticAccountMonthlyCashFlowPage>();
        services.AddTransient<StatisticCashFlowPage>();
        services.AddTransient<StatisticCategoryProgressionPage>();
        services.AddTransient<StatisticCategorySpreadingPage>();
        services.AddTransient<StatisticCategorySummaryPage>();
        services.AddTransient<AddBudgetPage>();
        services.AddTransient<EditBudgetPage>();
        services.AddTransient<BudgetListPage>();
        services.AddTransient<CategorySelectionControl>();
        services.AddTransient<DesktopAccountListPage>();
    }

    private static void RegisterAdapters(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IBrowserAdapter, BrowserAdapter>();
        serviceCollection.AddTransient<IConnectivityAdapter, ConnectivityAdapter>();
        serviceCollection.AddTransient<IEmailAdapter, EmailAdapter>();
        serviceCollection.AddTransient<ISettingsAdapter, SettingsAdapter>();
    }
}
