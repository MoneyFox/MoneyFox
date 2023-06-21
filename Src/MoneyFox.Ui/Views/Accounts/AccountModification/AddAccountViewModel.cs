namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using Core.Common.Interfaces;
using Core.Features._Legacy_.Accounts.CreateAccount;
using MediatR;
using Navigation;

internal sealed class AddAccountViewModel : ModifyAccountViewModel
{
    private readonly IMediator mediator;

    public AddAccountViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService) : base(
        dialogService: dialogService,
        mediator: mediator,
        navigationService: navigationService)
    {
        this.mediator = mediator;
    }

    protected override async Task SaveAccountAsync()
    {
        await mediator.Send(
            new CreateAccountCommand(
                name: SelectedAccountVm.Name,
                currentBalance: SelectedAccountVm.CurrentBalance,
                note: SelectedAccountVm.Note,
                isExcluded: SelectedAccountVm.IsExcluded));
    }
}
