using System;
using MoneyFox.Presentation.Views;
using NLog;
using Xamarin.Forms;

namespace MoneyFox.Presentation
{
    public partial class App 
    {
        public App()
        {
            StyleHelper.Init();
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogManager.GetCurrentClassLogger().Fatal(e);
            };

            ConfigureNavigation();
            
            var appShell = new AppShell();
            NavigationService.Initialize(appShell.Navigation);
            MainPage = new NavigationPage(appShell);
        }

        public void ConfigureNavigation()
        {
            NavigationService.Configure(ViewModelLocator.AccountList, typeof(AccountListPage));
            NavigationService.Configure(ViewModelLocator.PaymentList, typeof(PaymentListPage));
            NavigationService.Configure(ViewModelLocator.CategoryList, typeof(CategoryListPage));
            NavigationService.Configure(ViewModelLocator.SelectCategoryList, typeof(SelectCategoryPage));
            NavigationService.Configure(ViewModelLocator.AddAccount, typeof(AddAccountPage));
            NavigationService.Configure(ViewModelLocator.AddCategory, typeof(AddCategoryPage));
            NavigationService.Configure(ViewModelLocator.AddPayment, typeof(AddPaymentPage));
            NavigationService.Configure(ViewModelLocator.EditAccount, typeof(EditAccountPage));
            NavigationService.Configure(ViewModelLocator.EditCategory, typeof(EditCategoryPage));
            NavigationService.Configure(ViewModelLocator.EditPayment, typeof(EditPaymentPage));
            NavigationService.Configure(ViewModelLocator.SettingsBackgroundJob, typeof(BackgroundJobSettingsPage));
            NavigationService.Configure(ViewModelLocator.SettingsPersonalization, typeof(SettingsPersonalizationPage));
            NavigationService.Configure(ViewModelLocator.StatisticSelector, typeof(StatisticSelectorPage));
            NavigationService.Configure(ViewModelLocator.StatisticCashFlow, typeof(StatisticCashFlowPage));
            NavigationService.Configure(ViewModelLocator.StatisticCategorySpreading, typeof(StatisticCategorySpreadingPage));
            NavigationService.Configure(ViewModelLocator.StatisticCategorySummary, typeof(StatisticCategorySummaryPage));
            NavigationService.Configure(ViewModelLocator.About, typeof(AboutPage));
        }
    }
}
