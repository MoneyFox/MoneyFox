namespace MoneyFox
{

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
            Routing.RegisterRoute(route: ViewModelLocator.WelcomeViewRoute, type: typeof(WelcomePage));
            Routing.RegisterRoute(route: ViewModelLocator.CategoryIntroductionRoute, type: typeof(CategoryIntroductionPage));
            Routing.RegisterRoute(route: ViewModelLocator.SetupCompletionRoute, type: typeof(SetupCompletionPage));
            Routing.RegisterRoute(route: ViewModelLocator.DashboardRoute, type: typeof(DashboardPage));
            Routing.RegisterRoute(route: ViewModelLocator.AccountListRoute, type: typeof(AccountListPage));
            Routing.RegisterRoute(route: ViewModelLocator.AddAccountRoute, type: typeof(AddAccountPage));
            Routing.RegisterRoute(route: ViewModelLocator.EditAccountRoute, type: typeof(EditAccountPage));
            Routing.RegisterRoute(route: ViewModelLocator.BudgetListRoute, type: typeof(BudgetListPage));
            Routing.RegisterRoute(route: ViewModelLocator.PaymentListRoute, type: typeof(PaymentListPage));
            Routing.RegisterRoute(route: ViewModelLocator.CategoryListRoute, type: typeof(CategoryListPage));
            Routing.RegisterRoute(route: ViewModelLocator.SelectCategoryRoute, type: typeof(SelectCategoryPage));
            Routing.RegisterRoute(route: ViewModelLocator.AddCategoryRoute, type: typeof(AddCategoryPage));
            Routing.RegisterRoute(route: ViewModelLocator.AddPaymentRoute, type: typeof(AddPaymentPage));
            Routing.RegisterRoute(route: ViewModelLocator.BackupRoute, type: typeof(BackupPage));
            Routing.RegisterRoute(route: ViewModelLocator.SettingsRoute, type: typeof(SettingsPage));
            Routing.RegisterRoute(route: ViewModelLocator.AboutRoute, type: typeof(AboutPage));
            Routing.RegisterRoute(route: ViewModelLocator.StatisticCashFlowRoute, type: typeof(StatisticCashFlowPage));
            Routing.RegisterRoute(route: ViewModelLocator.StatisticAccountMonthlyCashFlowRoute, type: typeof(StatisticAccountMonthlyCashFlowPage));
            Routing.RegisterRoute(route: ViewModelLocator.StatisticCategoryProgressionRoute, type: typeof(StatisticCategoryProgressionPage));
            Routing.RegisterRoute(route: ViewModelLocator.StatisticCategorySpreadingRoute, type: typeof(StatisticCategorySpreadingPage));
            Routing.RegisterRoute(route: ViewModelLocator.StatisticCategorySummaryRoute, type: typeof(StatisticCategorySummaryPage));
            Routing.RegisterRoute(route: ViewModelLocator.StatisticSelectorRoute, type: typeof(StatisticSelectorPage));
            Routing.RegisterRoute(route: ViewModelLocator.PaymentForCategoryListRoute, type: typeof(PaymentForCategoryListPage));
            Routing.RegisterRoute(route: ViewModelLocator.BudgetListRoute, type: typeof(BudgetListPage));
            Routing.RegisterRoute(route: ViewModelLocator.AddBudgetRoute, type: typeof(AddBudgetPage));
        }
    }

}
