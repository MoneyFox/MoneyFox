namespace MoneyFox.Ui.Views.Budget;

using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.UseCases.BudgetCreation;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Interfaces;
using Core.Resources;
using MediatR;

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
            name: Name,
            spendingLimit: SpendingLimit,
            budgetTimeRange: TimeRange,
            categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

        _ = await sender.Send(query);
        _ = Messenger.Send(new ReloadMessage());
        await navigationService.GoBackFromModalAsync();
    }
}


