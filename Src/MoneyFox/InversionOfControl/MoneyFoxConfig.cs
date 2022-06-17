namespace MoneyFox.InversionOfControl
{

    using System.ComponentModel;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels.About;
    using ViewModels.Accounts;
    using ViewModels.Categories;
    using ViewModels.Dashboard;
    using ViewModels.DataBackup;
    using ViewModels.Dialogs;
    using ViewModels.OverflowMenu;
    using ViewModels.Payments;
    using ViewModels.Settings;
    using ViewModels.SetupAssistant;
    using ViewModels.Statistics;

    internal sealed class MoneyFoxConfig
    {
        public void Register(ServiceCollection serviceCollection)
        {
            RegisterViewModels(serviceCollection);
        }

        private static void RegisterViewModels(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AboutViewModel>();
            serviceCollection.AddTransient<AccountListViewModel>();
            serviceCollection.AddTransient<AddAccountViewModel>();
            serviceCollection.AddTransient<EditAccountViewModel>();
            serviceCollection.AddTransient<AddCategoryViewModel>();
            serviceCollection.AddTransient<CategoryListViewModel>();
            serviceCollection.AddTransient<EditCategoryViewModel>();
            serviceCollection.AddTransient<SelectCategoryViewModel>();
            serviceCollection.AddTransient<DashboardViewModel>();
            serviceCollection.AddTransient<BackupViewModel>();
            serviceCollection.AddTransient<SelectFilterDialogViewModel>();
            serviceCollection.AddTransient<OverflowMenuViewModel>();
            serviceCollection.AddTransient<AddPaymentViewModel>();
            serviceCollection.AddTransient<EditPaymentViewModel>();
            serviceCollection.AddTransient<PaymentListViewModel>();
            serviceCollection.AddTransient<SettingsViewModel>();
            serviceCollection.AddTransient<CategoryIntroductionViewModel>();
            serviceCollection.AddTransient<SetupCompletionViewModel>();
            serviceCollection.AddTransient<WelcomeViewModel>();
            serviceCollection.AddTransient<PaymentForCategoryListViewModel>();
            serviceCollection.AddTransient<StatisticAccountMonthlyCashFlowViewModel>();
            serviceCollection.AddTransient<StatisticCashFlowViewModel>();
            serviceCollection.AddTransient<StatisticCategoryProgressionViewModel>();
            serviceCollection.AddTransient<StatisticCategorySpreadingViewModel>();
            serviceCollection.AddTransient<StatisticCategorySummaryViewModel>();
            serviceCollection.AddTransient<StatisticSelectorViewModel>();
        }
    }

}
