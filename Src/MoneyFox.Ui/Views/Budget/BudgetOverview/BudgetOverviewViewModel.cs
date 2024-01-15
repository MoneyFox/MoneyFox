namespace MoneyFox.Ui.Views.Budget.BudgetOverview;

using System.Collections.ObjectModel;
using BudgetModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using MediatR;

internal sealed class BudgetOverviewViewModel(ISender sender, INavigationService navigationService) : NavigableViewModel
{
    private int budgetId;

    public AsyncRelayCommand GoToEditCommand => new(() => navigationService.GoTo<EditBudgetViewModel>());

    public ObservableCollection<BudgetPaymentViewModel> Payments { get; } = [];

    public override Task OnNavigatedAsync(object? parameter)
    {
        budgetId = Convert.ToInt32(parameter);

        return LoadData();
    }

    public override Task OnNavigatedBackAsync(object? parameter)
    {
        return LoadData();
    }

    private Task LoadData()
    {
        return Task.CompletedTask;
    }
}
