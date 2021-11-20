using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using MoneyFox.ViewModels.About;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Categories;
using MoneyFox.ViewModels.Dashboard;
using MoneyFox.ViewModels.DataBackup;
using MoneyFox.ViewModels.Dialogs;
using MoneyFox.ViewModels.Payments;
using MoneyFox.ViewModels.Settings;
using MoneyFox.ViewModels.Statistics;
using MoneyFox.Views.Accounts;
using MoneyFox.Views.Backup;
using MoneyFox.Views.Budget;
using MoneyFox.Views.Categories;
using MoneyFox.Views.Dashboard;
using MoneyFox.Views.Payments;
using MoneyFox.Views.Settings;
using MoneyFox.Views.SetupAssistant;
using MoneyFox.Views.Statistics;

namespace MoneyFox
{
    public class ViewModelLocator
    {
        public static void RegisterServices(ContainerBuilder registrations)
        {
            IContainer container = registrations.Build();

            if(container != null)
            {
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }

        // Routes
        public static string DashboardRoute => nameof(DashboardPage);
        public static string AccountListRoute => nameof(AccountListPage);
        public static string AddAccountRoute => nameof(AddAccountPage);
        public static string EditAccountRoute => nameof(EditAccountPage);
        public static string BudgetListRoute => nameof(BudgetListPage);
        public static string PaymentListRoute => nameof(PaymentListPage);
        public static string AddPaymentRoute => nameof(AddPaymentPage);
        public static string CategoryListRoute => nameof(CategoryListPage);
        public static string SelectCategoryRoute => nameof(SelectCategoryPage);
        public static string AddCategoryRoute => nameof(AddCategoryPage);
        public static string BackupRoute => nameof(BackupPage);
        public static string SettingsRoute => nameof(SettingsPage);
        public static string StatisticCashFlowRoute => nameof(StatisticCashFlowPage);
        public static string StatisticCategorySpreadingRoute => nameof(StatisticCategorySpreadingPage);
        public static string StatisticCategorySummaryRoute => nameof(StatisticCategorySummaryPage);
        public static string StatisticAccountMonthlyCashflowRoute => nameof(StatisticAccountMonthlyCashFlowPage);
        public static string StatisticCategoryProgressionRoute => nameof(StatisticCategoryProgressionPage);
        public static string StatisticSelectorRoute => nameof(StatisticSelectorPage);
        public static string PaymentForCategoryListRoute => nameof(PaymentForCategoryListPage);
        public static string WelcomeViewRoute => nameof(WelcomePage);
        public static string CategoryIntroductionRoute => nameof(CategoryIntroductionPage);
        public static string SetupCompletionRoute => nameof(SetupCompletionPage);

        // ViewModels
        public static DashboardViewModel DashboardViewModel => ServiceLocator.Current.GetInstance<DashboardViewModel>();

        public static AccountListViewModel AccountListViewModel =>
            ServiceLocator.Current.GetInstance<AccountListViewModel>();

        public static AddAccountViewModel AddAccountViewModel =>
            ServiceLocator.Current.GetInstance<AddAccountViewModel>();

        public static EditAccountViewModel EditAccountViewModel =>
            ServiceLocator.Current.GetInstance<EditAccountViewModel>();

        public static PaymentListViewModel PaymentListViewModel =>
            ServiceLocator.Current.GetInstance<PaymentListViewModel>();

        public static AddPaymentViewModel AddPaymentViewModel =>
            ServiceLocator.Current.GetInstance<AddPaymentViewModel>();

        public static EditPaymentViewModel EditPaymentViewModel =>
            ServiceLocator.Current.GetInstance<EditPaymentViewModel>();

        public static CategoryListViewModel CategoryListViewModel =>
            ServiceLocator.Current.GetInstance<CategoryListViewModel>();

        public static SelectCategoryViewModel SelectCategoryViewModel =>
            ServiceLocator.Current.GetInstance<SelectCategoryViewModel>();

        public static AddCategoryViewModel AddCategoryViewModel =>
            ServiceLocator.Current.GetInstance<AddCategoryViewModel>();

        public static EditCategoryViewModel EditCategoryViewModel =>
            ServiceLocator.Current.GetInstance<EditCategoryViewModel>();

        public static SelectDateRangeDialogViewModel SelectDateRangeDialogViewModel =>
            ServiceLocator.Current.GetInstance<SelectDateRangeDialogViewModel>();

        public static SelectFilterDialogViewModel SelectFilterDialogViewModel =>
            ServiceLocator.Current.GetInstance<SelectFilterDialogViewModel>();

        public static BackupViewModel BackupViewModel => ServiceLocator.Current.GetInstance<BackupViewModel>();

        public static StatisticCashFlowViewModel StatisticCashFlowViewModel =>
            ServiceLocator.Current.GetInstance<StatisticCashFlowViewModel>();

        public static StatisticCategoryProgressionViewModel StatisticCategoryProgressionViewModel =>
            ServiceLocator.Current.GetInstance<StatisticCategoryProgressionViewModel>();

        public static StatisticAccountMonthlyCashflowViewModel StatistcAccountMonthlyCashflowViewModel =>
            ServiceLocator.Current.GetInstance<StatisticAccountMonthlyCashflowViewModel>();

        public static StatisticCategorySpreadingViewModel StatisticCategorySpreadingViewModel =>
            ServiceLocator.Current.GetInstance<StatisticCategorySpreadingViewModel>();

        public static StatisticCategorySummaryViewModel StatisticCategorySummaryViewModel =>
            ServiceLocator.Current.GetInstance<StatisticCategorySummaryViewModel>();

        public static StatisticSelectorViewModel StatisticSelectorViewModel =>
            ServiceLocator.Current.GetInstance<StatisticSelectorViewModel>();

        public static PaymentForCategoryListViewModel PaymentForCategoryListViewModel =>
            ServiceLocator.Current.GetInstance<PaymentForCategoryListViewModel>();

        public static SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public static AboutViewModel AboutViewModel => ServiceLocator.Current.GetInstance<AboutViewModel>();
    }
}