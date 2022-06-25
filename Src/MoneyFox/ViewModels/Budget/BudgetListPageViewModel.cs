namespace MoneyFox.ViewModels.Budget;

using System.Collections.ObjectModel;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using Core.ApplicationCore.Queries.BudgetListLoading;
using Core.Common.Extensions;
using MediatR;
using Views.Budget;

internal sealed class BudgetListPageViewModel : BaseViewModel
{
    private readonly ISender sender;

    public BudgetListPageViewModel(ISender sender)
    {
        this.sender = sender;
    }

    public ObservableCollection<BudgetListViewModel> Budgets { get; } = new();

    public AsyncRelayCommand InitializeCommand => new(Initialize);

    public AsyncRelayCommand GoToAddBudgetCommand => new(GoToAddBudget);

    public AsyncRelayCommand<BudgetListViewModel> EditBudgetCommand => new(EditBudgetAsync);

    private async Task Initialize()
    {
        var budgetsListData = await sender.Send(new LoadBudgetListData.Query());
        Budgets.Clear();
        Budgets.AddRange(
            budgetsListData.Select(
                bld => new BudgetListViewModel
                {
                    Id = bld.Id,
                    Name = bld.Name,
                    SpendingLimit = bld.SpendingLimit,
                    CurrentSpending = bld.CurrentSpending
                }));
    }

    private static async Task GoToAddBudget()
    {
        await Shell.Current.GoToModalAsync(Routes.AddBudgetRoute);
    }

    private async Task EditBudgetAsync(BudgetListViewModel? selectedBudget)
    {
        if (selectedBudget == null)
        {
            return;
        }

        await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditBudgetPage(selectedBudget.Id)) { BarBackgroundColor = Colors.Transparent });
    }
}
