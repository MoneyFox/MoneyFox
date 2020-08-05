using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Budget;
using MoneyFox.ViewModels.Category;
using MoneyFox.ViewModels.Dashboard;
using MoneyFox.ViewModels.Payments;
using MoneyFox.Views.Accounts;
using MoneyFox.Views.Categories;
using MoneyFox.Views.Dashboard;
using MoneyFox.Views.Payments;

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
        public static string EditAccountRoute => $"Account/{nameof(EditAccountPage)}";
        public static string BudgetListRoute => $"Budget/{nameof(BudgetListPage)}";
        public static string PaymentListRoute => $"Payment/{nameof(PaymentListPage)}";
        public static string AddPaymentRoute => $"Payment/{nameof(AddPaymentPage)}";
        public static string CategoryListRoute => $"Category/{nameof(CategoryListPage)}";
        public static string AddCategoryRoute => $"Category/{nameof(AddCategoryPage)}";

        // ViewModels
        public static DashboardViewModel DashboardViewModel => ServiceLocator.Current.GetInstance<DashboardViewModel>();
        public static AccountListViewModel AccountListViewModel => ServiceLocator.Current.GetInstance<AccountListViewModel>();
        public static AddAccountViewModel AddAccountViewModel => ServiceLocator.Current.GetInstance<AddAccountViewModel>();
        public static EditAccountViewModel EditAccountViewModel => ServiceLocator.Current.GetInstance<EditAccountViewModel>();
        public static PaymentListViewModel PaymentListViewModel => ServiceLocator.Current.GetInstance<PaymentListViewModel>();
        public static AddPaymentViewModel AddPaymentViewModel => ServiceLocator.Current.GetInstance<AddPaymentViewModel>();
        public static EditPaymentViewModel EditPaymentViewModel => ServiceLocator.Current.GetInstance<EditPaymentViewModel>();
        public static CategoryListViewModel CategoryListViewModel => ServiceLocator.Current.GetInstance<CategoryListViewModel>();
        public static AddCategoryViewModel AddCategoryViewModel => ServiceLocator.Current.GetInstance<AddCategoryViewModel>();
        public static EditCategoryViewModel EditCategoryViewModel => ServiceLocator.Current.GetInstance<EditCategoryViewModel>();
    }
}
