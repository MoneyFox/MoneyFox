using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Budget;
using MoneyFox.ViewModels.Dashboard;
using MoneyFox.Views.Accounts;
using MoneyFox.Views.Dashboard;

namespace MoneyFox
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if(!ServiceLocator.IsLocationProviderSet && ViewModelBase.IsInDesignModeStatic)
            {
                RegisterServices(new ContainerBuilder());
            }
        }

        public static void RegisterServices(ContainerBuilder registrations)
        {
            IContainer container = registrations.Build();

            if(container != null)
            {
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }

        // Routes
        public static string DashboardRoute => $"Dashboard/{nameof(DashboardPage)}";
        public static string AccountListRoute => $"Account/{nameof(AccountListPage)}";
        public static string AddAccountRoute => $"Account/{nameof(AddAccountPage)}";
        public static string BudgetListRoute => $"Budget/{nameof(BudgetListPage)}";


        // ViewModels
        public static DashboardViewModel DashboardViewModel => ServiceLocator.Current.GetInstance<DashboardViewModel>();
        public static AccountListViewModel AccountListViewModel => ServiceLocator.Current.GetInstance<AccountListViewModel>();
        public static AddAccountViewModel AddAccountViewModel => ServiceLocator.Current.GetInstance<AddAccountViewModel>();

    }
}
