using MoneyFox.ViewModels.Budget;
using MoneyFox.Views.Accounts;
using MoneyFox.Views.Backup;
using MoneyFox.Views.Categories;
using MoneyFox.Views.Dashboard;
using MoneyFox.Views.Payments;
using MoneyFox.Views.Settings;
using MoneyFox.Views.Statistics;
using Xamarin.Forms;

namespace MoneyFox
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(ViewModelLocator.DashboardRoute, typeof(DashboardPage));
            Routing.RegisterRoute(ViewModelLocator.AccountListRoute, typeof(AccountListPage));
            Routing.RegisterRoute(ViewModelLocator.AddAccountRoute, typeof(AddAccountPage));
            Routing.RegisterRoute(ViewModelLocator.EditAccountRoute, typeof(EditAccountPage));
            Routing.RegisterRoute(ViewModelLocator.BudgetListRoute, typeof(BudgetListPage));
            Routing.RegisterRoute(ViewModelLocator.PaymentListRoute, typeof(PaymentListPage));
            Routing.RegisterRoute(ViewModelLocator.CategoryListRoute, typeof(CategoryListPage));
            Routing.RegisterRoute(ViewModelLocator.SelectCategoryRoute, typeof(SelectedCategoryPage));
            Routing.RegisterRoute(ViewModelLocator.AddCategoryRoute, typeof(AddCategoryPage));
            Routing.RegisterRoute(ViewModelLocator.AddPaymentRoute, typeof(AddPaymentPage));
            Routing.RegisterRoute(ViewModelLocator.BackupRoute, typeof(BackupPage));
            Routing.RegisterRoute(ViewModelLocator.SettingsRoute, typeof(SettingsPage));
            Routing.RegisterRoute(ViewModelLocator.StatisticCashFlowRoute, typeof(StatisticCashFlowPage));
            Routing.RegisterRoute(ViewModelLocator.StatisticCategorySpreadingRoute, typeof(StatisticCategorySpreadingPage));
            Routing.RegisterRoute(ViewModelLocator.StatisticCategorySummaryRoute, typeof(StatisticCategorySummaryPage));
            Routing.RegisterRoute(ViewModelLocator.StatisticSelectorRoute, typeof(StatisticSelectorPage));
        }
    }
}
