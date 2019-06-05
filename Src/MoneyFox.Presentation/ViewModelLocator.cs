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
        public AccountListViewModel AccountListVm => SimpleIoc.Default.GetInstance<AccountListViewModel>();
        public CategoryListViewModel CategoryListVm => SimpleIoc.Default.GetInstance<CategoryListViewModel>();
        public SelectCategoryListViewModel SelectCategoryListVm => SimpleIoc.Default.GetInstance<SelectCategoryListViewModel>();
        public AddAccountViewModel AddAccountVm => SimpleIoc.Default.GetInstance<AddAccountViewModel>();
        public AddCategoryViewModel AddCategoryVm => SimpleIoc.Default.GetInstance<AddCategoryViewModel>();
        public AddPaymentViewModel AddPaymentVm => SimpleIoc.Default.GetInstance<AddPaymentViewModel>();
        public EditAccountViewModel EditAccountVm => SimpleIoc.Default.GetInstance<EditAccountViewModel>();
        public EditCategoryViewModel EditCategoryVm => SimpleIoc.Default.GetInstance<EditCategoryViewModel>();
        public EditPaymentViewModel EditPaymentVm => SimpleIoc.Default.GetInstance<EditPaymentViewModel>();
        public BackupViewModel BackupVm => SimpleIoc.Default.GetInstance<BackupViewModel>();
        public SettingsBackgroundJobViewModel SettingsBackgroundVm => SimpleIoc.Default.GetInstance<SettingsBackgroundJobViewModel>();
        public SettingsPersonalizationViewModel SettingsPersonalizationVm => SimpleIoc.Default.GetInstance<SettingsPersonalizationViewModel>();
        public StatisticSelectorViewModel StatisticSelectorVm => SimpleIoc.Default.GetInstance<StatisticSelectorViewModel>();
        public StatisticCashFlowViewModel StatisticCashFlowVm => SimpleIoc.Default.GetInstance<StatisticCashFlowViewModel>();
        public StatisticCategorySpreadingViewModel StatisticCategorySpreadingVm => SimpleIoc.Default.GetInstance<StatisticCategorySpreadingViewModel>();
        public StatisticCategorySummaryViewModel StatisticCategorySummaryVm => SimpleIoc.Default.GetInstance<StatisticCategorySummaryViewModel>();
        public AboutViewModel AboutVm => SimpleIoc.Default.GetInstance<AboutViewModel>();
    }
}
