using MoneyFox.ViewModels.Budget;
using MoneyFox.Views.Accounts;
using MoneyFox.Views.Dashboard;
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
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(ViewModelLocator.AccountListRoute, typeof(AccountListPage));
            Routing.RegisterRoute(nameof(BudgetListPage), typeof(BudgetListPage));
        }
    }
}
