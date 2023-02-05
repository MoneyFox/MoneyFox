namespace MoneyFox.Ui;

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
using Views.Payments;
using Views.Payments.PaymentModification;
using Views.Settings;
using Views.Setup;
using Views.Setup.SelectCurrency;
using Views.Statistics.CashFlow;
using Views.Statistics.CategoryProgression;
using Views.Statistics.CategorySpreading;
using Views.Statistics.CategorySummary;
using Views.Statistics.MonthlyAccountCashFlow;
using Views.Statistics.Selector;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(route: Routes.DashboardRoute, type: typeof(DashboardPage));
        Routing.RegisterRoute(route: Routes.AddAccountRoute, type: typeof(AddAccountPage));
        Routing.RegisterRoute(route: Routes.AccountListRoute, type: typeof(AccountListPage));
        Routing.RegisterRoute(route: Routes.EditAccountRoute, type: typeof(EditAccountPage));
        Routing.RegisterRoute(route: Routes.BudgetListRoute, type: typeof(BudgetListPage));
        Routing.RegisterRoute(route: Routes.PaymentListRoute, type: typeof(PaymentListPage));
        Routing.RegisterRoute(route: Routes.CategoryListRoute, type: typeof(CategoryListPage));
        Routing.RegisterRoute(route: Routes.SelectCategoryRoute, type: typeof(SelectCategoryPage));
        Routing.RegisterRoute(route: Routes.AddCategoryRoute, type: typeof(AddCategoryPage));
        Routing.RegisterRoute(route: Routes.EditCategoryRoute, type: typeof(EditCategoryPage));
        Routing.RegisterRoute(route: Routes.AddPaymentRoute, type: typeof(AddPaymentPage));
        Routing.RegisterRoute(route: Routes.EditPaymentRoute, type: typeof(EditPaymentPage));
        Routing.RegisterRoute(route: Routes.BackupRoute, type: typeof(BackupPage));
        Routing.RegisterRoute(route: Routes.SettingsRoute, type: typeof(SettingsPage));
        Routing.RegisterRoute(route: Routes.AboutRoute, type: typeof(AboutPage));
        Routing.RegisterRoute(route: Routes.StatisticCashFlowRoute, type: typeof(StatisticCashFlowPage));
        Routing.RegisterRoute(route: Routes.StatisticAccountMonthlyCashFlowRoute, type: typeof(StatisticAccountMonthlyCashFlowPage));
        Routing.RegisterRoute(route: Routes.StatisticCategoryProgressionRoute, type: typeof(StatisticCategoryProgressionPage));
        Routing.RegisterRoute(route: Routes.StatisticCategorySpreadingRoute, type: typeof(StatisticCategorySpreadingPage));
        Routing.RegisterRoute(route: Routes.StatisticCategorySummaryRoute, type: typeof(StatisticCategorySummaryPage));
        Routing.RegisterRoute(route: Routes.StatisticSelectorRoute, type: typeof(StatisticSelectorPage));
        Routing.RegisterRoute(route: Routes.PaymentForCategoryListRoute, type: typeof(PaymentForCategoryListPage));
        Routing.RegisterRoute(route: Routes.BudgetListRoute, type: typeof(BudgetListPage));
        Routing.RegisterRoute(route: Routes.AddBudgetRoute, type: typeof(AddBudgetPage));
        Routing.RegisterRoute(route: Routes.EditBudgetRoute, type: typeof(EditBudgetPage));

        // Setup
        Routing.RegisterRoute(route: Routes.WelcomeViewRoute, type: typeof(WelcomePage));
        Routing.RegisterRoute(route: Routes.CurrencyIntroductionRoute, type: typeof(SetupCurrencyPage));
        Routing.RegisterRoute(route: Routes.SetupAccountsRoute, type: typeof(SetupAccountPage));
        Routing.RegisterRoute(route: Routes.CategoryIntroductionRoute, type: typeof(SetupCategoryPage));
        Routing.RegisterRoute(route: Routes.SetupCompletionRoute, type: typeof(SetupCompletionPage));
    }
}
