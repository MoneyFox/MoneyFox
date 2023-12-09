namespace MoneyFox.Ui.Common.Navigation;

using Domain.Exceptions;
using Exceptions;
using Extensions;
using Views.About;
using Views.Accounts.AccountList;
using Views.Accounts.AccountModification;
using Views.Backup;
using Views.Budget;
using Views.Budget.BudgetModification;
using Views.Categories;
using Views.Categories.CategorySelection;
using Views.Categories.ModifyCategory;
using Views.Dashboard;
using Views.Payments.PaymentList;
using Views.Payments.PaymentModification;
using Views.Settings;
using Views.Setup;
using Views.Setup.SelectCurrency;
using Views.Statistics.CashFlow;
using Views.Statistics.CategoryProgression;
using Views.Statistics.CategorySpreading;
using Views.Statistics.CategorySummary;
using Views.Statistics.MonthlyAccountCashFlow;
using Views.Statistics.Selector;

internal class ViewLocator(IServiceProvider serviceProvider) : IViewLocator
{
    private static readonly List<(Type ViewModelType, Type ViewType)> viewLocatorDictionary =
    [
        (typeof(DashboardViewModel), typeof(DashboardPage)), (typeof(AccountListViewModel), typeof(AccountListPage)),
        (typeof(AddAccountViewModel), typeof(AddAccountPage)), (typeof(EditAccountViewModel), typeof(EditAccountPage)),
        (typeof(BudgetListViewModel), typeof(BudgetListPage)), (typeof(PaymentListViewModel), typeof(PaymentListPage)),
        (typeof(AddPaymentViewModel), typeof(AddPaymentPage)), (typeof(EditPaymentViewModel), typeof(EditPaymentPage)),
        (typeof(CategoryListViewModel), typeof(CategoryListPage)), (typeof(SelectCategoryViewModel), typeof(SelectCategoryPage)),
        (typeof(AddCategoryViewModel), typeof(AddCategoryPage)), (typeof(EditCategoryViewModel), typeof(EditCategoryPage)),
        (typeof(BackupViewModel), typeof(BackupPage)), (typeof(SettingsViewModel), typeof(SettingsPage)), (typeof(AboutViewModel), typeof(AboutPage)),
        (typeof(StatisticCashFlowViewModel), typeof(StatisticCashFlowPage)),
        (typeof(StatisticCategorySpreadingViewModel), typeof(StatisticCategorySpreadingPage)),
        (typeof(StatisticCategorySummaryViewModel), typeof(StatisticCategorySummaryPage)),
        (typeof(StatisticAccountMonthlyCashFlowViewModel), typeof(StatisticAccountMonthlyCashFlowPage)),
        (typeof(StatisticCategoryProgressionViewModel), typeof(StatisticCategoryProgressionPage)),
        (typeof(StatisticSelectorViewModel), typeof(SelectCategoryPage)), (typeof(PaymentForCategoryListViewModel), typeof(PaymentForCategoryListPage)),
        (typeof(AddBudgetViewModel), typeof(AddBudgetPage)), (typeof(EditBudgetViewModel), typeof(EditBudgetPage)),
        (typeof(WelcomeViewModel), typeof(WelcomePage)), (typeof(SetupCurrencyViewModel), typeof(SetupCurrencyPage)),
        (typeof(SetupAccountsViewModel), typeof(SetupAccountPage)), (typeof(SetupCategoryViewModel), typeof(SetupCategoryPage)),
        (typeof(SetupCompletionViewModel), typeof(SetupCompletionPage)), (typeof(AboutViewModel), typeof(AboutPage)),
        (typeof(SettingsViewModel), typeof(SettingsPage))
    ];

    private static readonly List<(Type ViewModelType, Type ViewType)> desktopViewLocatorDictionary =
    [
        (typeof(DashboardViewModel), typeof(DashboardPage)), (typeof(AccountListViewModel), typeof(DesktopAccountListPage)),
        (typeof(AddAccountViewModel), typeof(AddAccountPage)), (typeof(EditAccountViewModel), typeof(EditAccountPage)),
        (typeof(BudgetListViewModel), typeof(BudgetListPage)), (typeof(PaymentListViewModel), typeof(PaymentListPage)),
        (typeof(AddPaymentViewModel), typeof(AddPaymentPage)), (typeof(EditPaymentViewModel), typeof(EditPaymentPage)),
        (typeof(CategoryListViewModel), typeof(CategoryListPage)), (typeof(SelectCategoryViewModel), typeof(SelectCategoryPage)),
        (typeof(AddCategoryViewModel), typeof(AddCategoryPage)), (typeof(EditCategoryViewModel), typeof(EditCategoryPage)),
        (typeof(BackupViewModel), typeof(BackupPage)), (typeof(SettingsViewModel), typeof(SettingsPage)), (typeof(AboutViewModel), typeof(AboutPage)),
        (typeof(StatisticCashFlowViewModel), typeof(StatisticCashFlowPage)),
        (typeof(StatisticCategorySpreadingViewModel), typeof(StatisticCategorySpreadingPage)),
        (typeof(StatisticCategorySummaryViewModel), typeof(StatisticCategorySummaryPage)),
        (typeof(StatisticAccountMonthlyCashFlowViewModel), typeof(StatisticAccountMonthlyCashFlowPage)),
        (typeof(StatisticCategoryProgressionViewModel), typeof(StatisticCategoryProgressionPage)),
        (typeof(StatisticSelectorViewModel), typeof(SelectCategoryPage)), (typeof(PaymentForCategoryListViewModel), typeof(PaymentForCategoryListPage)),
        (typeof(AddBudgetViewModel), typeof(AddBudgetPage)), (typeof(EditBudgetViewModel), typeof(EditBudgetPage)),
        (typeof(WelcomeViewModel), typeof(WelcomePage)), (typeof(SetupCurrencyViewModel), typeof(SetupCurrencyPage)),
        (typeof(SetupAccountsViewModel), typeof(SetupAccountPage)), (typeof(SetupCategoryViewModel), typeof(SetupCategoryPage)),
        (typeof(SetupCompletionViewModel), typeof(SetupCompletionPage)), (typeof(AboutViewModel), typeof(AboutPage)),
        (typeof(SettingsViewModel), typeof(SettingsPage))
    ];

    public IBindablePage GetViewFor<TViewModel>() where TViewModel : NavigableViewModel
    {
        var viewModel = serviceProvider.GetService<TViewModel>();
        if (viewModel is null)
        {
            throw new ResolveDependencyException(typeof(TViewModel));
        }

        var view = (IBindablePage?)serviceProvider.GetService(FindViewByViewModel(typeof(TViewModel)));
        if (view is null)
        {
            throw new NavigationException(typeof(TViewModel));
        }

        view.BindingContext = viewModel;

        return view;
    }

    public IBindablePage GetView<TView>() where TView : class, IBindablePage
    {
        var view = (IBindablePage?)serviceProvider.GetService(typeof(TView));
        if (view is null)
        {
            throw new ResolveDependencyException(typeof(TView));
        }

        var viewModel = serviceProvider.GetService(FindViewModelByView(typeof(TView)));
        if (viewModel is null)
        {
            throw new ViewModelForPageNotFoundException(typeof(TView));
        }

        view.BindingContext = viewModel;

        return view;
    }

    public Type GetViewTypeFor<TViewModel>() where TViewModel : NavigableViewModel
    {
        var view = FindViewByViewModel(typeof(TViewModel));
        if (view is null)
        {
            throw new NoPageForViewModelRegisteredException(typeof(TViewModel));
        }

        return view;
    }

    private static Type FindViewModelByView(Type viewType)
    {
        var locatorDictionary = DeviceInfo.Current.Idiom.UseDesktopPage() ? desktopViewLocatorDictionary : viewLocatorDictionary;
        foreach (var pair in locatorDictionary)
        {
            if (pair.ViewType == viewType)
            {
                return pair.ViewModelType;
            }
        }

        throw new NoViewModelForPageRegisteredException(viewType);
    }

    private static Type FindViewByViewModel(Type viewModelType)
    {
        var locatorDictionary = DeviceInfo.Current.Idiom.UseDesktopPage() ? desktopViewLocatorDictionary : viewLocatorDictionary;
        foreach (var pair in locatorDictionary)
        {
            if (pair.ViewModelType == viewModelType)
            {
                return pair.ViewType;
            }
        }

        throw new NoPageForViewModelRegisteredException(viewModelType);
    }
}
