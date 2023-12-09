namespace MoneyFox.Ui.Views.Budget;

using System.Collections.ObjectModel;
using BudgetModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Extensions;
using Core.Queries.BudgetList;
using MediatR;

public sealed class BudgetListViewModel(ISender sender, INavigationService navigationService) : NavigableViewModel
{
    public bool HasBudgets => Budgets.Any();

    public ObservableCollection<BudgetListItemViewModel> Budgets { get; } = new();

    public decimal BudgetedAmount => Budgets.Sum(b => b.MonthlyBudget);
    public decimal SpentAmount => Budgets.Sum(b => b.MonthlySpending);

    public AsyncRelayCommand InitializeCommand => new(InitializeAsync);

    public AsyncRelayCommand GoToAddBudgetCommand => new(() => navigationService.GoTo<AddBudgetViewModel>());

    public AsyncRelayCommand<BudgetListItemViewModel> EditBudgetCommand => new(EditBudgetAsync);

    public override Task OnNavigatedAsync(object? parameter)
    {
        return InitializeAsync();
    }

    public override Task OnNavigatedBackAsync(object? parameter)
    {
        return InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var budgetsListData = await sender.Send(new LoadBudgetDataForList.Query());
        Budgets.Clear();
        Budgets.AddRange(
            budgetsListData.OrderBy(bld => bld.Name)
                .Select(
                    bld => new BudgetListItemViewModel
                    {
                        Id = bld.Id,
                        Name = bld.Name,
                        SpendingLimit = bld.SpendingLimit,
                        CurrentSpending = bld.CurrentSpending,
                        MonthlyBudget = bld.MonthlyBudget,
                        MonthlySpending = bld.MonthlySpending
                    }));

        OnPropertyChanged(nameof(BudgetedAmount));
        OnPropertyChanged(nameof(HasBudgets));
    }

    private Task EditBudgetAsync(BudgetListItemViewModel? selectedBudget)
    {
        return selectedBudget == null ? Task.CompletedTask : navigationService.GoTo<EditBudgetViewModel>(selectedBudget.Id);
    }
}
