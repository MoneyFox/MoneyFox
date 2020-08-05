using MoneyFox.ViewModels.Budget;
using MoneyFox.Views.Accounts;
using MoneyFox.Views.Categories;
using MoneyFox.Views.Dashboard;
using MoneyFox.Views.Payments;
using Xamarin.Forms;

namespace MoneyFox
{
    public partial class AppShell : Xamarin.Forms.Shell
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
            Routing.RegisterRoute(ViewModelLocator.AddCategoryRoute, typeof(AddCategoryPage));
        }
    }
}
