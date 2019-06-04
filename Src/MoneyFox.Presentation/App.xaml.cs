using System;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Presentation.Views;
using MoneyFox.Views;
using NLog;
using Xamarin.Forms;

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
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);

            var mainPage = new NavigationPage(new MainPage());
            navigationService.Initialize(mainPage);
            MainPage = mainPage;
        }

        public NavigationService ConfigureNavigation()
        {
            var nav = new NavigationService();

            nav.Configure(Locator.AccountList, typeof(AccountListPage));
            nav.Configure(Locator.CategoryList, typeof(CategoryListPage));
            nav.Configure(Locator.SelectCategoryList, typeof(SelectCategoryPage));
            nav.Configure(Locator.AddAccount, typeof(AddAccountPage));
            nav.Configure(Locator.AddCategory, typeof(AddCategoryPage));
            nav.Configure(Locator.AddPayment, typeof(AddPaymentPage));
            nav.Configure(Locator.EditAccount, typeof(EditAccountPage));
            nav.Configure(Locator.EditCategory, typeof(EditCategoryPage));
            nav.Configure(Locator.EditPayment, typeof(EditPaymentPage));
            nav.Configure(Locator.SettingsBackgroundJob, typeof(BackgroundJobSettingsPage));
            nav.Configure(Locator.SettingsPersonalization, typeof(SettingsPersonalizationPage));
            nav.Configure(Locator.StatisticSelector, typeof(StatisticSelectorPage));
            nav.Configure(Locator.StatisticCashFlow, typeof(StatisticCashFlowPage));
            nav.Configure(Locator.StatisticCategorySpreading, typeof(StatisticCategorySpreadingPage));
            nav.Configure(Locator.StatisticCategorySummary, typeof(StatisticCategorySummaryPage));
            nav.Configure(Locator.About, typeof(AboutPage));

            return nav;
        }
    }
}
