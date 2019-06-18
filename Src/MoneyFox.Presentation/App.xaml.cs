using System;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Presentation.Views;
using MoneyFox.Views;
using NLog;

namespace MoneyFox.Presentation
{
    public partial class App 
    {
        public App ()
        {
            StyleHelper.Init();
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogManager.GetCurrentClassLogger().Fatal(e);
            };

            var navigationService = ConfigureNavigation();

            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            }

            var appShell = new AppShell();
            navigationService.Initialize(appShell.Navigation);
            MainPage = appShell;
        }

        public NavigationService ConfigureNavigation()
        {
            var nav = new NavigationService();

            nav.Configure(ViewModelLocator.AccountList, typeof(AccountListPage));
            nav.Configure(ViewModelLocator.PaymentList, typeof(PaymentListPage));
            nav.Configure(ViewModelLocator.CategoryList, typeof(CategoryListPage));
            nav.Configure(ViewModelLocator.SelectCategoryList, typeof(SelectCategoryPage));
            nav.Configure(ViewModelLocator.AddAccount, typeof(AddAccountPage));
            nav.Configure(ViewModelLocator.AddCategory, typeof(AddCategoryPage));
            nav.Configure(ViewModelLocator.AddPayment, typeof(AddPaymentPage));
            nav.Configure(ViewModelLocator.EditAccount, typeof(EditAccountPage));
            nav.Configure(ViewModelLocator.EditCategory, typeof(EditCategoryPage));
            nav.Configure(ViewModelLocator.EditPayment, typeof(EditPaymentPage));
            nav.Configure(ViewModelLocator.SettingsBackgroundJob, typeof(BackgroundJobSettingsPage));
            nav.Configure(ViewModelLocator.SettingsPersonalization, typeof(SettingsPersonalizationPage));
            nav.Configure(ViewModelLocator.StatisticSelector, typeof(StatisticSelectorPage));
            nav.Configure(ViewModelLocator.StatisticCashFlow, typeof(StatisticCashFlowPage));
            nav.Configure(ViewModelLocator.StatisticCategorySpreading, typeof(StatisticCategorySpreadingPage));
            nav.Configure(ViewModelLocator.StatisticCategorySummary, typeof(StatisticCategorySummaryPage));
            nav.Configure(ViewModelLocator.About, typeof(AboutPage));

            return nav;
        }
    }
}
