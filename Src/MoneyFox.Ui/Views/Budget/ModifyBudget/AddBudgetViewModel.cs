namespace MoneyFox.Ui.Views.Budget.ModifyBudget;

using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Core.Features.BudgetCreation;
using MoneyFox.Core.Interfaces;

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
        CreateBudget.Command query = new(
            name: Name,
            spendingLimit: SpendingLimit,
            budgetTimeRange: TimeRange,
            categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

        _ = await sender.Send(query);
        _ = Messenger.Send(new ReloadMessage());
        await navigationService.GoBackFromModalAsync();
    }
}
