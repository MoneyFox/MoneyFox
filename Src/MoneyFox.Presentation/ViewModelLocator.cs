using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MoneyFox.Presentation.Views;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.ServiceLayer.ViewModels.Statistic;
using MoneyFox.Views;

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
            if (ViewModelBase.IsInDesignModeStatic)
            {
                //builder.RegisterModule<FakeServiceModule>();
            } 
            else
            {
                container = registrations?.Build();
            }
            
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }

        public static string MainPage => nameof(MainPage);
        public static string AccountList => nameof(AccountList);
        public static string CategoryList => nameof(CategoryList);
        public static string SelectCategoryList => nameof(SelectCategoryList);
        public static string AddAccount => nameof(AddCategoryPage);
        public static string AddCategory => nameof(AddCategoryPage);
        public static string AddPayment => nameof(AddPaymentPage);
        public static string EditAccount => nameof(EditAccountPage);
        public static string EditCategory => nameof(EditCategoryPage);
        public static string EditPayment => nameof(EditPaymentPage);
        public static string Backup => nameof(BackupPage);
        public static string SettingsBackgroundJob => nameof(BackgroundJobSettingsPage);
        public static string SettingsPersonalization => nameof(SettingsPersonalizationPage);
        public static string StatisticSelector => nameof(StatisticSelectorPage);
        public static string StatisticCashFlow => nameof(StatisticCashFlowPage);
        public static string StatisticCategorySpreading => nameof(StatisticCategorySpreadingPage);
        public static string StatisticCategorySummary => nameof(StatisticCategorySummaryPage);
        public static string About => nameof(AboutPage);

        public MainViewModel MainVm => SimpleIoc.Default.GetInstance<MainViewModel>();
        public AccountListViewModel AccountListVm =>  ServiceLocator.Current.GetInstance<AccountListViewModel>();
        public CategoryListViewModel CategoryListVm => ServiceLocator.Current.GetInstance<CategoryListViewModel>();
        public PaymentListViewModel PaymentListVm => ServiceLocator.Current.GetInstance<PaymentListViewModel>();
        public SelectCategoryListViewModel SelectCategoryListVm => ServiceLocator.Current.GetInstance<SelectCategoryListViewModel>();
        public AddAccountViewModel AddAccountVm => ServiceLocator.Current.GetInstance<AddAccountViewModel>();
        public AddCategoryViewModel AddCategoryVm => ServiceLocator.Current.GetInstance<AddCategoryViewModel>();
        public AddPaymentViewModel AddPaymentVm => ServiceLocator.Current.GetInstance<AddPaymentViewModel>();
        public EditAccountViewModel EditAccountVm => ServiceLocator.Current.GetInstance<EditAccountViewModel>();
        public EditCategoryViewModel EditCategoryVm => ServiceLocator.Current.GetInstance<EditCategoryViewModel>();
        public EditPaymentViewModel EditPaymentVm => ServiceLocator.Current.GetInstance<EditPaymentViewModel>();
        public BackupViewModel BackupVm => ServiceLocator.Current.GetInstance<BackupViewModel>();
        public SettingsViewModel SettingsVm => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public SettingsBackgroundJobViewModel SettingsBackgroundVm => ServiceLocator.Current.GetInstance<SettingsBackgroundJobViewModel>();
        public SettingsPersonalizationViewModel SettingsPersonalizationVm => ServiceLocator.Current.GetInstance<SettingsPersonalizationViewModel>();
        public StatisticSelectorViewModel StatisticSelectorVm => ServiceLocator.Current.GetInstance<StatisticSelectorViewModel>();
        public StatisticCashFlowViewModel StatisticCashFlowVm => ServiceLocator.Current.GetInstance<StatisticCashFlowViewModel>();
        public StatisticCategorySpreadingViewModel StatisticCategorySpreadingVm => ServiceLocator.Current.GetInstance<StatisticCategorySpreadingViewModel>();
        public StatisticCategorySummaryViewModel StatisticCategorySummaryVm => ServiceLocator.Current.GetInstance<StatisticCategorySummaryViewModel>();
        public AboutViewModel AboutVm => ServiceLocator.Current.GetInstance<AboutViewModel>();
    }
}
