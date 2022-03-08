namespace MoneyFox
{
    using MoneyFox.Views.About;
    using Views.Accounts;
    using Views.Backup;
    using Views.Budget;
    using Views.Categories;
    using Views.Dashboard;
    using Views.Payments;
    using Views.Settings;
    using Views.SetupAssistant;
    using Views.Statistics;
    using Xamarin.Forms;

    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute(ViewModelLocator.WelcomeViewRoute, typeof(WelcomePage));
            Routing.RegisterRoute(ViewModelLocator.CategoryIntroductionRoute, typeof(CategoryIntroductionPage));
            Routing.RegisterRoute(ViewModelLocator.SetupCompletionRoute, typeof(SetupCompletionPage));

            Routing.RegisterRoute(ViewModelLocator.DashboardRoute, typeof(DashboardPage));
            Routing.RegisterRoute(ViewModelLocator.AccountListRoute, typeof(AccountListPage));
            Routing.RegisterRoute(ViewModelLocator.AddAccountRoute, typeof(AddAccountPage));
            Routing.RegisterRoute(ViewModelLocator.EditAccountRoute, typeof(EditAccountPage));
            Routing.RegisterRoute(ViewModelLocator.BudgetListRoute, typeof(BudgetListPage));
            Routing.RegisterRoute(ViewModelLocator.PaymentListRoute, typeof(PaymentListPage));
            Routing.RegisterRoute(ViewModelLocator.CategoryListRoute, typeof(CategoryListPage));
            Routing.RegisterRoute(ViewModelLocator.SelectCategoryRoute, typeof(SelectCategoryPage));
            Routing.RegisterRoute(ViewModelLocator.AddCategoryRoute, typeof(AddCategoryPage));
            Routing.RegisterRoute(ViewModelLocator.AddPaymentRoute, typeof(AddPaymentPage));
            Routing.RegisterRoute(ViewModelLocator.BackupRoute, typeof(BackupPage));
            Routing.RegisterRoute(ViewModelLocator.SettingsRoute, typeof(SettingsPage));
            Routing.RegisterRoute(ViewModelLocator.AboutRoute, typeof(AboutPage));
            Routing.RegisterRoute(ViewModelLocator.StatisticCashFlowRoute, typeof(StatisticCashFlowPage));
            Routing.RegisterRoute(
                ViewModelLocator.StatisticAccountMonthlyCashflowRoute,
                typeof(StatisticAccountMonthlyCashFlowPage));
            Routing.RegisterRoute(
                ViewModelLocator.StatisticCategoryProgressionRoute,
                typeof(StatisticCategoryProgressionPage));
            Routing.RegisterRoute(
                ViewModelLocator.StatisticCategorySpreadingRoute,
                typeof(StatisticCategorySpreadingPage));
            Routing.RegisterRoute(ViewModelLocator.StatisticCategorySummaryRoute, typeof(StatisticCategorySummaryPage));
            Routing.RegisterRoute(ViewModelLocator.StatisticSelectorRoute, typeof(StatisticSelectorPage));
            Routing.RegisterRoute(ViewModelLocator.PaymentForCategoryListRoute, typeof(PaymentForCategoryListPage));
        }
    }
}