namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using Common.Navigation;
using Core.Common.Interfaces;
using Core.Features.BudgetCreation;
using MediatR;

internal sealed class AddBudgetViewModel : ModifyBudgetViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    public AddBudgetViewModel(ISender sender, INavigationService navigationService, IDialogService dialogService) : base(
        navigationService: navigationService,
        sender: sender,
        dialogService: dialogService)
    {
        this.sender = sender;
        this.navigationService = navigationService;
    }

    protected override async Task SaveAsync()
    {
        CreateBudget.Command query = new(
            Name: Name,
            SpendingLimit: new(SpendingLimit),
            BudgetInterval: new(NumberOfMonths),
            Categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

        await sender.Send(query);
        await navigationService.GoBack();
    }
}
