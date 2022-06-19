namespace MoneyFox.ViewModels.Accounts
{

    using System.Threading.Tasks;
    using Core.Commands.Accounts.CreateAccount;
    using Core.Common.Interfaces;
    using MediatR;

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

}
