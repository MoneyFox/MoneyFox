namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using Common.Navigation;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Accounts.CreateAccount;
using MediatR;
using Resources.Strings;

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

    public override string Title => Translations.AddAccountTitle;

    protected override Task SaveAccountAsync()
    {
        return mediator.Send(
            new CreateAccountCommand(
                name: SelectedAccountVm.Name,
                currentBalance: SelectedAccountVm.CurrentBalance,
                note: SelectedAccountVm.Note ?? string.Empty,
                isExcluded: SelectedAccountVm.IsExcluded));
    }
}
