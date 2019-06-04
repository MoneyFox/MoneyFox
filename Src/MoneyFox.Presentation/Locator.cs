using GalaSoft.MvvmLight.Ioc;
using MoneyFox.Presentation.Views;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.ServiceLayer.ViewModels.Statistic;
using MoneyFox.Views;

namespace MoneyFox.Presentation
{
    public class Locator
    {
        public Locator()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AccountListViewModel>();
            SimpleIoc.Default.Register<CategoryListViewModel>();
            SimpleIoc.Default.Register<SelectCategoryListViewModel>();
            SimpleIoc.Default.Register<PaymentListViewModel>();
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddCategoryViewModel>();
            SimpleIoc.Default.Register<AddPaymentViewModel>();
            SimpleIoc.Default.Register<EditAccountViewModel>();
            SimpleIoc.Default.Register<EditCategoryViewModel>();
            SimpleIoc.Default.Register<EditPaymentViewModel>();
            SimpleIoc.Default.Register<BackupViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<SettingsBackgroundJobViewModel>();
            SimpleIoc.Default.Register<SettingsPersonalizationViewModel>();
            SimpleIoc.Default.Register<StatisticCashFlowViewModel>();
            SimpleIoc.Default.Register<StatisticCategorySpreadingViewModel>();
            SimpleIoc.Default.Register<StatisticCategorySummaryViewModel>();
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
    }
}
