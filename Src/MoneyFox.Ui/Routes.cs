namespace MoneyFox.Ui;

using Views.About;
using Views.Accounts;
using Views.Backup;
using Views.Budget;
using Views.Categories;
using Views.Categories.ModifyCategory;
using Views.Dashboard;
using Views.Payments;
using Views.Settings;
using Views.SetupAssistant;
using Views.Statistics.CashFlow;
using Views.Statistics.CategoryProgression;
using Views.Statistics.CategorySpreading;
using Views.Statistics.CategorySummary;
using Views.Statistics.MonthlyAccountCashFlow;
using Views.Statistics.Selector;

public static class Routes
{
    public static string DashboardRoute => nameof(DashboardPage);
    public static string AccountListRoute => nameof(AccountListPage);
    public static string AddAccountRoute => nameof(AddAccountPage);
    public static string EditAccountRoute => nameof(EditAccountPage);
    public static string BudgetListRoute => nameof(BudgetListPage);
    public static string PaymentListRoute => nameof(PaymentListPage);
    public static string AddPaymentRoute => nameof(AddPaymentPage);
    public static string EditPaymentRoute => nameof(EditPaymentPage);
    public static string CategoryListRoute => nameof(CategoryListPage);
    public static string SelectCategoryRoute => nameof(SelectCategoryPage);
    public static string AddCategoryRoute => nameof(AddCategoryPage);
    public static string EditCategoryRoute => nameof(EditCategoryPage);
    public static string BackupRoute => nameof(BackupPage);
    public static string SettingsRoute => nameof(SettingsPage);
    public static string AboutRoute => nameof(AboutPage);
    public static string StatisticCashFlowRoute => nameof(StatisticCashFlowPage);
    public static string StatisticCategorySpreadingRoute => nameof(StatisticCategorySpreadingPage);
    public static string StatisticCategorySummaryRoute => nameof(StatisticCategorySummaryPage);
    public static string StatisticAccountMonthlyCashFlowRoute => nameof(StatisticAccountMonthlyCashFlowPage);
    public static string StatisticCategoryProgressionRoute => nameof(StatisticCategoryProgressionPage);
    public static string StatisticSelectorRoute => nameof(StatisticSelectorPage);
    public static string PaymentForCategoryListRoute => nameof(PaymentForCategoryListPage);
    public static string WelcomeViewRoute => nameof(WelcomePage);
    public static string CategoryIntroductionRoute => nameof(CategoryIntroductionPage);
    public static string SetupCompletionRoute => nameof(SetupCompletionPage);
    public static string AddBudgetRoute => nameof(AddBudgetPage);
    public static string EditBudgetRoute => nameof(EditBudgetPage);
}
