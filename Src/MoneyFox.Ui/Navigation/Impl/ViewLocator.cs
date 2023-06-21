// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewLocator.cs" company="The Silly Company">
//   The Silly Company 2016. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MoneyFox.Ui.Navigation.Impl;

using Common.Exceptions;
using Views;
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
using Views.Statistics.CashFlow;
using Views.Statistics.CategoryProgression;
using Views.Statistics.CategorySpreading;
using Views.Statistics.CategorySummary;
using Views.Statistics.MonthlyAccountCashFlow;
using Views.Statistics.Selector;

public class ViewLocator : IViewLocator
{
    private static readonly List<(Type ViewModelType, Type ViewType)> viewLocatorDictionary = new()
    {
        (typeof(DashboardViewModel), typeof(DashboardPage)),
        (typeof(AddAccountViewModel), typeof(AddAccountPage)),
        (typeof(AccountListViewModel), typeof(AccountListPage)),
        (typeof(EditAccountViewModel), typeof(EditAccountPage)),
        (typeof(BudgetListViewModel), typeof(BudgetListPage)),
        (typeof(PaymentListViewModel), typeof(PaymentListPage)),
        (typeof(CategoryListViewModel), typeof(CategoryListPage)),
        (typeof(SelectCategoryViewModel), typeof(SelectCategoryPage)),
        (typeof(AddCategoryViewModel), typeof(AddCategoryPage)),
        (typeof(EditCategoryViewModel), typeof(EditCategoryPage)),
        (typeof(AddPaymentViewModel), typeof(AddPaymentPage)),
        (typeof(EditPaymentViewModel), typeof(EditPaymentPage)),
        (typeof(BackupViewModel), typeof(BackupPage)),
        (typeof(SettingsViewModel), typeof(SettingsPage)),
        (typeof(AboutViewModel), typeof(AboutPage)),
        (typeof(StatisticCashFlowViewModel), typeof(StatisticCashFlowPage)),
        (typeof(StatisticAccountMonthlyCashFlowViewModel), typeof(StatisticAccountMonthlyCashFlowPage)),
        (typeof(StatisticCategoryProgressionViewModel), typeof(StatisticCategoryProgressionPage)),
        (typeof(StatisticCategorySpreadingViewModel), typeof(StatisticCategorySpreadingPage)),
        (typeof(StatisticCategorySummaryViewModel), typeof(StatisticCategorySummaryPage)),
        (typeof(StatisticSelectorViewModel), typeof(StatisticSelectorPage)),
        (typeof(PaymentForCategoryListViewModel), typeof(PaymentForCategoryListPage)),
        (typeof(BudgetListViewModel), typeof(BudgetListPage)),
        (typeof(AddBudgetViewModel), typeof(AddBudgetPage)),
        (typeof(EditBudgetViewModel), typeof(EditBudgetPage))
    };

    private readonly IServiceProvider serviceProvider;

    public ViewLocator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IBindablePage GetViewFor<TViewModel>() where TViewModel : BasePageViewModel
    {
        var viewModel = App.GetViewModel<TViewModel>();
        var view = serviceProvider.GetService(FindViewByViewModel(typeof(TViewModel))) as IBindablePage
            ?? throw new ResolveViewException(typeof(TViewModel));
        view.BindingContext = viewModel;

        return view;
    }

    public IBindablePage GetView<TView>() where TView : class, IBindablePage
    {
        var view = (IBindablePage)serviceProvider.GetService(typeof(TView))!;
        var viewModel = serviceProvider.GetService(FindViewModelByView(typeof(TView)))
                        ?? throw new ResolveViewException(typeof(TView));
        view.BindingContext = viewModel;

        return view;
    }

    /// <summary>
    ///     Gets the view type matching the given view model.
    /// </summary>
    /// <typeparam name="TViewModel">
    ///     The view model type.
    /// </typeparam>
    /// <returns>
    /// </returns>
    public Type GetViewTypeFor<TViewModel>() where TViewModel : BasePageViewModel
    {
        return FindViewByViewModel(typeof(TViewModel));
    }

    private static Type FindViewModelByView(Type viewType)
    {
        foreach (var pair in viewLocatorDictionary)
        {
            if (pair.ViewType == viewType)
            {
                return pair.ViewModelType;
            }
        }

        throw new FindViewModelForViewException(viewType);
    }

    private static Type FindViewByViewModel(Type viewModelType)
    {
        foreach (var pair in viewLocatorDictionary)
        {
            if (pair.ViewModelType == viewModelType)
            {
                return pair.ViewType;
            }
        }

        throw new FindViewForViewModelException(viewModelType);
    }
}
