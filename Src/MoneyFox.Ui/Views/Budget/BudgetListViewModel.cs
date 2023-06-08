namespace MoneyFox.Ui.Views.Budget;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Extensions;
using Core.Queries.BudgetList;
using MediatR;

public sealed class BudgetListViewModel : BasePageViewModel, IRecipient<BudgetsChangedMessage>
{
    private readonly ISender sender;

    public BudgetListViewModel(ISender sender)
    {
        this.sender = sender;
    }

    public bool HasBudgets => Budgets.Any();

    public ObservableCollection<BudgetListItemViewModel> Budgets { get; } = new();

    public decimal BudgetedAmount => Budgets.Sum(b => b.MonthlyBudget);

    public AsyncRelayCommand InitializeCommand => new(Initialize);

    public AsyncRelayCommand GoToAddBudgetCommand => new(GoToAddBudget);

    public AsyncRelayCommand<BudgetListItemViewModel> EditBudgetCommand => new(EditBudgetAsync);

    public void Receive(BudgetsChangedMessage message)
    {
        Initialize().GetAwaiter().GetResult();
    }

    private async Task Initialize()
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
                        MonthlyBudget = bld.MonthlyBudget
                    }));

        OnPropertyChanged(nameof(BudgetedAmount));
        OnPropertyChanged(nameof(HasBudgets));
    }

    private static async Task GoToAddBudget()
    {
        await Shell.Current.GoToAsync(Routes.AddBudgetRoute);
    }

    private async Task EditBudgetAsync(BudgetListItemViewModel? selectedBudget)
    {
        if (selectedBudget == null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{Routes.EditBudgetRoute}?budgetId={selectedBudget.Id}");
    }
}
