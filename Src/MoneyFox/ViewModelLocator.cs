using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.Presentation.Views;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Backup;
using MoneyFox.ViewModels.Budget;
using MoneyFox.ViewModels.Categories;
using MoneyFox.ViewModels.Dashboard;
using MoneyFox.ViewModels.Dialogs;
using MoneyFox.ViewModels.Payments;
using MoneyFox.ViewModels.Statistics;
using MoneyFox.Views;
using MoneyFox.Views.Accounts;
using MoneyFox.Views.Categories;
using MoneyFox.Views.Dashboard;
using MoneyFox.Views.Payments;
using MoneyFox.Views.Settings;

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
        public static string SelectCategoryRoute => $"Category/{nameof(SelectedCategoryPage)}";
        public static string AddCategoryRoute => $"Category/{nameof(AddCategoryPage)}";
        public static string BackupRoute => $"Backup/Show{nameof(BackupPage)}";
        public static string SettingsRoute => $"Settings/Show{nameof(SettingsPage)}";
        public static string StatisticCashFlowRoute => $"Settings/Show{nameof(StatisticCashFlowPage)}";
        public static string StatisticCategorySpreadingRoute => $"Settings/Show{nameof(StatisticCategorySpreadingPage)}";
        public static string StatisticCategorySummaryRoute => $"Settings/Show{nameof(StatisticCategorySummaryPage)}";
        public static string StatisticSelectorRoute => $"Settings/Show{nameof(StatisticSelectorPage)}";
        public static string PaymentForCategoryListRoute => $"Settings/Show{nameof(PaymentForCategoryListPage)}";

        // ViewModels
        public static DashboardViewModel DashboardViewModel => ServiceLocator.Current.GetInstance<DashboardViewModel>();
        public static AccountListViewModel AccountListViewModel => ServiceLocator.Current.GetInstance<AccountListViewModel>();
        public static AddAccountViewModel AddAccountViewModel => ServiceLocator.Current.GetInstance<AddAccountViewModel>();
        public static EditAccountViewModel EditAccountViewModel => ServiceLocator.Current.GetInstance<EditAccountViewModel>();
        public static PaymentListViewModel PaymentListViewModel => ServiceLocator.Current.GetInstance<PaymentListViewModel>();
        public static AddPaymentViewModel AddPaymentViewModel => ServiceLocator.Current.GetInstance<AddPaymentViewModel>();
        public static EditPaymentViewModel EditPaymentViewModel => ServiceLocator.Current.GetInstance<EditPaymentViewModel>();
        public static CategoryListViewModel CategoryListViewModel => ServiceLocator.Current.GetInstance<CategoryListViewModel>();
        public static SelectCategoryViewModel SelectCategoryViewModel => ServiceLocator.Current.GetInstance<SelectCategoryViewModel>();
        public static AddCategoryViewModel AddCategoryViewModel => ServiceLocator.Current.GetInstance<AddCategoryViewModel>();
        public static EditCategoryViewModel EditCategoryViewModel => ServiceLocator.Current.GetInstance<EditCategoryViewModel>();
        public static SelectDateRangeDialogViewModel SelectDateRangeDialogViewModel => ServiceLocator.Current.GetInstance<SelectDateRangeDialogViewModel>();
        public static SelectFilterDialogViewModel SelectFilterDialogViewModel => ServiceLocator.Current.GetInstance<SelectFilterDialogViewModel>();
        public static BackupViewModel BackupViewModel => ServiceLocator.Current.GetInstance<BackupViewModel>();
        public static StatisticCashFlowViewModel StatisticCashFlowViewModel => ServiceLocator.Current.GetInstance<StatisticCashFlowViewModel>();
        public static StatisticCategorySpreadingViewModel StatisticCategorySpreadingViewModel => ServiceLocator.Current.GetInstance<StatisticCategorySpreadingViewModel>();
        public static StatisticCategorySummaryViewModel StatisticCategorySummaryViewModel => ServiceLocator.Current.GetInstance<StatisticCategorySummaryViewModel>();
        public static StatisticSelectorViewModel StatisticSelectorViewModel => ServiceLocator.Current.GetInstance<StatisticSelectorViewModel>();
        public static PaymentForCategoryListViewModel PaymentForCategoryListViewModel => ServiceLocator.Current.GetInstance<PaymentForCategoryListViewModel>();
    }
}
