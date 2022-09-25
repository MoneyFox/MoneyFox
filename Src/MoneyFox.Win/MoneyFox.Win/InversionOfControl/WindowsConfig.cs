namespace MoneyFox.Win.InversionOfControl;

using Common.Mapping;
using Core.Common.Interfaces;
using Core.Interfaces;
using Core.InversionOfControl;
using Microsoft.Extensions.DependencyInjection;
using MoneyFox.Win.Infrastructure.InversionOfControl;
using Services;
using ViewModels;
using ViewModels.About;
using ViewModels.Accounts;
using ViewModels.Categories;
using ViewModels.DataBackup;
using ViewModels.Payments;
using ViewModels.Settings;
using ViewModels.Statistics;
using ViewModels.Statistics.StatisticCategorySummary;
using INavigationService = Services.INavigationService;

internal sealed class WindowsConfig
{
    public void Register(IServiceCollection serviceCollection)
    {
        RegisterViewModels(serviceCollection);
        RegisterWindowsServices(serviceCollection);
        serviceCollection.AddSingleton(_ => AutoMapperFactory.Create());
        new InfrastructureWinConfig().Register(serviceCollection);
        new CoreConfig().Register(serviceCollection);
    }

    private static void RegisterWindowsServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INavigationService, NavigationService>();
        serviceCollection.AddTransient<IDialogService, DialogService>();
        serviceCollection.AddTransient<IToastService, ToastService>();
        serviceCollection.AddTransient<IAppInformation, WindowsAppInformation>();
        serviceCollection.AddTransient<IStoreOperations, MarketplaceOperations>();
        serviceCollection.AddTransient<IBalanceCalculationService, BalanceCalculationService>();
        serviceCollection.AddTransient<IFileStore, WindowsFileStore>();
    }

    private static void RegisterViewModels(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<AboutViewModel>();
        serviceCollection.AddTransient<AccountListViewModel>();
        serviceCollection.AddTransient<AddAccountViewModel>();
        serviceCollection.AddTransient<EditAccountViewModel>();
        serviceCollection.AddTransient<AddCategoryViewModel>();
        serviceCollection.AddTransient<CategoryListViewModel>();
        serviceCollection.AddTransient<EditCategoryViewModel>();
        serviceCollection.AddTransient<SelectCategoryListViewModel>();
        serviceCollection.AddTransient<BackupViewModel>();
        serviceCollection.AddTransient<SelectFilterDialogViewModel>();
        serviceCollection.AddTransient<AddPaymentViewModel>();
        serviceCollection.AddTransient<EditPaymentViewModel>();
        serviceCollection.AddTransient<PaymentListViewModel>();
        serviceCollection.AddTransient<SettingsViewModel>();
        serviceCollection.AddTransient<WindowsSettingsViewModel>();
        serviceCollection.AddTransient<StatisticCategorySummaryViewModel>();
        serviceCollection.AddTransient<StatisticAccountMonthlyCashflowViewModel>();
        serviceCollection.AddTransient<StatisticCashFlowViewModel>();
        serviceCollection.AddTransient<StatisticCategoryProgressionViewModel>();
        serviceCollection.AddTransient<StatisticCategorySpreadingViewModel>();
        serviceCollection.AddTransient<StatisticCategorySummaryViewModel>();
        serviceCollection.AddTransient<StatisticSelectorViewModel>();
        serviceCollection.AddTransient<IncomeExpenseBalanceViewModel>();
        serviceCollection.AddTransient<SelectFilterDialogViewModel>();
        serviceCollection.AddTransient<ShellViewModel>();
    }
}
