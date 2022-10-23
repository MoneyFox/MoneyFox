namespace MoneyFox.Ui.Views.Budget;

using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Resources;

internal sealed class AddBudgetViewModel : ModifyBudgetViewModel
{
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    public AddBudgetViewModel(ISender sender, INavigationService navigationService, IDialogService dialogService) : base(navigationService: navigationService)
    {
        this.sender = sender;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
    }

    protected override async Task SaveBudgetAsync()
    {
        if (SelectedBudget.SpendingLimit <= 0)
        {
            await dialogService.ShowMessageAsync(title: Strings.InvalidSpendingLimitTitle, message: Strings.InvalidSpendingLimitMessage);

            return;
        }

        var query = new CreateBudget.Command(
            name: SelectedBudget.Name,
            spendingLimit: SelectedBudget.SpendingLimit,
            budgetTimeRange: BudgetTimeRange.YearToDate,
            categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

        await sender.Send(query);
        Messenger.Send(new ReloadMessage());
        await navigationService.GoBackFromModalAsync();
    }
}

