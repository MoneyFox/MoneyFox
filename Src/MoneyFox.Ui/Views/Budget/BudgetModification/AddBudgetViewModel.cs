namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using CommunityToolkit.Mvvm.Messaging;
using Core.Features.BudgetCreation;
using MediatR;

internal sealed class AddBudgetViewModel : ModifyBudgetViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    public AddBudgetViewModel(ISender sender, INavigationService navigationService) : base(navigationService: navigationService, sender: sender)
    {
        this.sender = sender;
        this.navigationService = navigationService;
    }

    protected override async Task SaveBudgetAsync()
    {
        CreateBudget.Command query = new(
            Name: Name,
            SpendingLimit: new(SpendingLimit),
            BudgetInterval: new(NumberOfMonths),
            Categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

        await sender.Send(query);
        await navigationService.GoBackFromModalAsync();
        _ = Messenger.Send(new BudgetsChangedMessage());
    }
}
