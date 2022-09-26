namespace MoneyFox.Ui.ViewModels.Budget;

using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Core.Interfaces;

internal sealed class AddBudgetViewModel : ModifyBudgetViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    public AddBudgetViewModel(ISender sender, INavigationService navigationService) : base(navigationService: navigationService)
    {
        this.sender = sender;
        this.navigationService = navigationService;
    }

    protected override async Task SaveBudgetAsync()
    {
        var query = new CreateBudget.Command(
            name: SelectedBudget.Name,
            spendingLimit: SelectedBudget.SpendingLimit,
            budgetTimeRange: BudgetTimeRange.YearToDate,
            categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

        await sender.Send(query);
        Messenger.Send(new ReloadMessage());
        await navigationService.GoBackFromModal();
    }
}
