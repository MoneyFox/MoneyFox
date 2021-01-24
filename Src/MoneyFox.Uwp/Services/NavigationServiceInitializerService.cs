using MoneyFox.Presentation.ViewModels.Statistic;
using MoneyFox.Ui.Shared.ViewModels.Backup;
using MoneyFox.Ui.Shared.ViewModels.Settings;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using MoneyFox.Uwp.ViewModels;
using MoneyFox.Uwp.ViewModels.Accounts;
using MoneyFox.Uwp.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels.Payments;
using MoneyFox.Uwp.ViewModels.Settings;
using MoneyFox.Uwp.ViewModels.Statistic;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;
using MoneyFox.Uwp.Views;
using MoneyFox.Uwp.Views.Accounts;
using MoneyFox.Uwp.Views.Categories;
using MoneyFox.Uwp.Views.Payments;
using MoneyFox.Uwp.Views.Settings;
using MoneyFox.Uwp.Views.Statistics;
using MoneyFox.Uwp.Views.Statistics.StatisticCategorySummary;
using MoneyFox.ViewModels.Statistics;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    public static class NavigationServiceInitializerService
    {
        public static void Initialize()
        {
            NavigationService.Register<AccountListViewModel, AccountListView>();
            NavigationService.Register<PaymentListViewModel, PaymentListView>();
            NavigationService.Register<AddPaymentViewModel, AddPaymentView>();
            NavigationService.Register<EditPaymentViewModel, EditPaymentView>();
            NavigationService.Register<CategoryListViewModel, CategoryListView>();
            NavigationService.Register<SettingsViewModel, SettingsHostView>();
            NavigationService.Register<StatisticCashFlowViewModel, StatisticCashFlowView>();
            NavigationService.Register<StatisticAccountMonthlyCashflowViewModel, StatisticAccountMonthlyCashflowView>();
            NavigationService.Register<StatisticCategorySpreadingViewModel, StatisticCategorySpreadingView>();
            NavigationService.Register<StatisticCategorySummaryViewModel, StatisticCategorySummaryView>();
            NavigationService.Register<StatisticSelectorViewModel, StatisticSelectorView>();
            NavigationService.Register<BackupViewModel, BackupView>();
            NavigationService.Register<WindowsSettingsViewModel, SettingsHostView>();
        }
    }
}
