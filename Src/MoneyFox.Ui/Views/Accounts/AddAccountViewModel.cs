using MoneyFox.Ui.ViewModels.Accounts;

namespace MoneyFox.Ui.Views.Accounts;

using MediatR;
using MoneyFox.Core.Commands.Accounts.CreateAccount;
using MoneyFox.Core.Common.Interfaces;

internal sealed class AddAccountViewModel : ModifyAccountViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;

    public AddAccountViewModel(IMediator mediator, IDialogService dialogService) : base(dialogService: dialogService, mediator: mediator)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
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
