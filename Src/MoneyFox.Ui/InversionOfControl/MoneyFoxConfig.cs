namespace MoneyFox.Ui.InversionOfControl;

using Common.Services;
using Infrastructure.Adapters;
using Mapping;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.InversionOfControl;
using MoneyFox.Infrastructure.InversionOfControl;
using MoneyFox.Ui.Views.About;
using MoneyFox.Ui.Views.Backup;
using MoneyFox.Ui.Views.Dashboard;
using ViewModels.Accounts;
using ViewModels.Budget;
using ViewModels.Categories;
using ViewModels.Dialogs;
using ViewModels.OverflowMenu;
using ViewModels.Payments;
using ViewModels.Settings;
using ViewModels.SetupAssistant;
using ViewModels.Statistics;

public sealed class MoneyFoxConfig
{
    public void Register(ServiceCollection serviceCollection)
    {
        RegisterServices(serviceCollection);
        RegisterViewModels(serviceCollection);
        RegisterAdapters(serviceCollection);

        _ = serviceCollection.AddSingleton(_ => AutoMapperFactory.Create());
        new CoreConfig().Register(serviceCollection);
        InfrastructureConfig.Register(serviceCollection);
    }

    private static void RegisterServices(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<IDialogService, DialogService>();
        _ = serviceCollection.AddTransient<INavigationService, NavigationService>();
        _ = serviceCollection.AddTransient<IToastService, ToastService>();
    }

    private static void RegisterViewModels(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<AboutViewModel>();
        _ = serviceCollection.AddTransient<AccountListViewModel>();
        _ = serviceCollection.AddTransient<AddAccountViewModel>();
        _ = serviceCollection.AddTransient<EditAccountViewModel>();
        _ = serviceCollection.AddTransient<AddCategoryViewModel>();
        _ = serviceCollection.AddTransient<CategoryListViewModel>();
        _ = serviceCollection.AddTransient<EditCategoryViewModel>();
        _ = serviceCollection.AddTransient<SelectCategoryViewModel>();
        _ = serviceCollection.AddTransient<DashboardViewModel>();
        _ = serviceCollection.AddTransient<BackupViewModel>();
        _ = serviceCollection.AddTransient<OverflowMenuViewModel>();
        _ = serviceCollection.AddTransient<AddPaymentViewModel>();
        _ = serviceCollection.AddTransient<EditPaymentViewModel>();
        _ = serviceCollection.AddTransient<PaymentListViewModel>();
        _ = serviceCollection.AddTransient<SettingsViewModel>();
        _ = serviceCollection.AddTransient<CategoryIntroductionViewModel>();
        _ = serviceCollection.AddTransient<SetupCompletionViewModel>();
        _ = serviceCollection.AddTransient<WelcomeViewModel>();
        _ = serviceCollection.AddTransient<PaymentForCategoryListViewModel>();
        _ = serviceCollection.AddTransient<StatisticAccountMonthlyCashFlowViewModel>();
        _ = serviceCollection.AddTransient<StatisticCashFlowViewModel>();
        _ = serviceCollection.AddTransient<StatisticCategoryProgressionViewModel>();
        _ = serviceCollection.AddTransient<StatisticCategorySpreadingViewModel>();
        _ = serviceCollection.AddTransient<StatisticCategorySummaryViewModel>();
        _ = serviceCollection.AddTransient<StatisticSelectorViewModel>();
        _ = serviceCollection.AddTransient<SelectDateRangeDialogViewModel>();
        _ = serviceCollection.AddTransient<SelectFilterDialogViewModel>();
        _ = serviceCollection.AddTransient<AddBudgetViewModel>();
        _ = serviceCollection.AddTransient<EditBudgetViewModel>();
        _ = serviceCollection.AddTransient<BudgetListViewModel>();
    }

    private static void RegisterAdapters(ServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<IBrowserAdapter, BrowserAdapter>();
        _ = serviceCollection.AddTransient<IConnectivityAdapter, ConnectivityAdapter>();
        _ = serviceCollection.AddTransient<IEmailAdapter, EmailAdapter>();
        _ = serviceCollection.AddTransient<ISettingsAdapter, SettingsAdapter>();
    }
}
