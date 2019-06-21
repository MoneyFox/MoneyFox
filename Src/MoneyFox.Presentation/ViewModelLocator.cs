using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.Presentation.Views;
using MoneyFox.Views;
using MainViewModel = MoneyFox.Presentation.ViewModels.MainViewModel;

namespace MoneyFox.Presentation
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                RegisterServices();
            }
        }

        public static void RegisterServices(ContainerBuilder registrations= null)
        {
            IContainer container = null;

            // you only need this if-clause if you'd like to use design-time data which is only supported on XAML-based platforms
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                container = registrations?.Build();
            }

            if (container != null)
            {
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }

        public static string MainPage => nameof(MainPage);
        public static string AccountList => nameof(AccountListPage);
        public static string PaymentList => nameof(PaymentListPage);
        public static string CategoryList => nameof(CategoryList);
        public static string SelectCategoryList => nameof(SelectCategoryPage);
        public static string AddAccount => nameof(AddCategoryPage);
        public static string AddCategory => nameof(AddCategoryPage);
        public static string AddPayment => nameof(AddPaymentPage);
        public static string EditAccount => nameof(EditAccountPage);
        public static string EditCategory => nameof(EditCategoryPage);
        public static string EditPayment => nameof(EditPaymentPage);
        public static string Backup => nameof(BackupPage);
        public static string Settings => nameof(SettingsPage);
        public static string SettingsBackgroundJob => nameof(BackgroundJobSettingsPage);
        public static string SettingsPersonalization => nameof(SettingsPersonalizationPage);
        public static string StatisticSelector => nameof(StatisticSelectorPage);
        public static string StatisticCashFlow => nameof(StatisticCashFlowPage);
        public static string StatisticCategorySpreading => nameof(StatisticCategorySpreadingPage);
        public static string StatisticCategorySummary => nameof(StatisticCategorySummaryPage);
        public static string About => nameof(AboutPage);

        public static MainViewModel MainVm => ServiceLocator.Current.GetInstance<MainViewModel>();
        public static AccountListViewModel AccountListVm =>  ServiceLocator.Current.GetInstance<AccountListViewModel>();
        public static CategoryListViewModel CategoryListVm => ServiceLocator.Current.GetInstance<CategoryListViewModel>();
        public static PaymentListViewModel PaymentListVm => ServiceLocator.Current.GetInstance<PaymentListViewModel>();
        public static SelectCategoryListViewModel SelectCategoryListVm => ServiceLocator.Current.GetInstance<SelectCategoryListViewModel>();
        public static AddAccountViewModel AddAccountVm => ServiceLocator.Current.GetInstance<AddAccountViewModel>();
        public static AddCategoryViewModel AddCategoryVm => ServiceLocator.Current.GetInstance<AddCategoryViewModel>();
        public static AddPaymentViewModel AddPaymentVm => ServiceLocator.Current.GetInstance<AddPaymentViewModel>();
        public static EditAccountViewModel EditAccountVm => ServiceLocator.Current.GetInstance<EditAccountViewModel>();
        public static EditCategoryViewModel EditCategoryVm => ServiceLocator.Current.GetInstance<EditCategoryViewModel>();
        public static EditPaymentViewModel EditPaymentVm => ServiceLocator.Current.GetInstance<EditPaymentViewModel>();
        public static BackupViewModel BackupVm => ServiceLocator.Current.GetInstance<BackupViewModel>();
        public static SettingsViewModel SettingsVm => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public static SettingsBackgroundJobViewModel SettingsBackgroundVm => ServiceLocator.Current.GetInstance<SettingsBackgroundJobViewModel>();
        public static SettingsPersonalizationViewModel SettingsPersonalizationVm => ServiceLocator.Current.GetInstance<SettingsPersonalizationViewModel>();
        public static StatisticSelectorViewModel StatisticSelectorVm => ServiceLocator.Current.GetInstance<StatisticSelectorViewModel>();
        public static StatisticCashFlowViewModel StatisticCashFlowVm => ServiceLocator.Current.GetInstance<StatisticCashFlowViewModel>();
        public static StatisticCategorySpreadingViewModel StatisticCategorySpreadingVm => ServiceLocator.Current.GetInstance<StatisticCategorySpreadingViewModel>();
        public static StatisticCategorySummaryViewModel StatisticCategorySummaryVm => ServiceLocator.Current.GetInstance<StatisticCategorySummaryViewModel>();
        public static AboutViewModel AboutVm => ServiceLocator.Current.GetInstance<AboutViewModel>();
    }
}
