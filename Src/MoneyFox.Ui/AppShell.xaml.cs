namespace MoneyFox.Ui;

using Views.About;
using Views.Accounts;
using Views.Backup;
using Views.Budget;
using Views.Categories;
using Views.Dashboard;
using Views.Payments;
using Views.Settings;
using Views.SetupAssistant;
using Views.Statistics;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(route: Routes.WelcomeViewRoute, type: typeof(WelcomePage));
        Routing.RegisterRoute(route: Routes.CategoryIntroductionRoute, type: typeof(CategoryIntroductionPage));
        Routing.RegisterRoute(route: Routes.SetupCompletionRoute, type: typeof(SetupCompletionPage));
        Routing.RegisterRoute(route: Routes.DashboardRoute, type: typeof(DashboardPage));
        Routing.RegisterRoute(route: Routes.AccountListRoute, type: typeof(AccountListPage));
        Routing.RegisterRoute(route: Routes.AddAccountRoute, type: typeof(AddAccountPage));
        Routing.RegisterRoute(route: Routes.EditAccountRoute, type: typeof(EditAccountPage));
        Routing.RegisterRoute(route: Routes.BudgetListRoute, type: typeof(BudgetListPage));
        Routing.RegisterRoute(route: Routes.PaymentListRoute, type: typeof(PaymentListPage));
        Routing.RegisterRoute(route: Routes.CategoryListRoute, type: typeof(CategoryListPage));
        Routing.RegisterRoute(route: Routes.SelectCategoryRoute, type: typeof(SelectCategoryPage));
        Routing.RegisterRoute(route: Routes.AddCategoryRoute, type: typeof(AddCategoryPage));
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
    }
}
