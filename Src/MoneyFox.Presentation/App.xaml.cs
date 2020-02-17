using System;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.Views;
using NLog;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MoneyFox.Presentation
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            Material.Init(this, "Material.Configuration");

            AppDomain.CurrentDomain.UnhandledException += (s, e) => { LogManager.GetCurrentClassLogger().Fatal(e.ExceptionObject); };

            ConfigureNavigation();

            var appShell = new AppShell();
            NavigationService.Initialize(appShell.Navigation);
            MainPage = new NavigationPage(appShell);
            ThemeManager.LoadTheme();
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
            NavigationService.Configure(ViewModelLocator.Backup, typeof(BackupPage));

            // Statistics
            NavigationService.Configure(ViewModelLocator.StatisticSelector, typeof(StatisticSelectorPage));
            NavigationService.Configure(ViewModelLocator.StatisticCashFlow, typeof(StatisticCashFlowPage));
            NavigationService.Configure(ViewModelLocator.StatisticCategorySpreading, typeof(StatisticCategorySpreadingPage));
            NavigationService.Configure(ViewModelLocator.StatisticCategorySummary, typeof(StatisticCategorySummaryPage));

            // Settings
            NavigationService.Configure(ViewModelLocator.SettingsRegional, typeof(SettingsRegionalPage));
            NavigationService.Configure(ViewModelLocator.SettingsBackgroundJob, typeof(BackgroundJobSettingsPage));
            NavigationService.Configure(ViewModelLocator.SettingsPersonalization, typeof(SettingsPersonalizationPage));
            NavigationService.Configure(ViewModelLocator.About, typeof(AboutPage));
        }
    }
}
