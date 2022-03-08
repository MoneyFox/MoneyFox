namespace MoneyFox.ViewModels.Accounts
{
    using Core.Commands.Accounts.CreateAccount;
    using Core.Common.Interfaces;
    using MediatR;
    using System.Threading.Tasks;

    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly IDialogService dialogService;
        private readonly IMediator mediator;

        public AddAccountViewModel(
            IMediator mediator,
            IDialogService dialogService)
            : base(dialogService, mediator)
        {
            this.mediator = mediator;
            this.dialogService = dialogService;
        }

        protected override async Task SaveAccountAsync() =>
            await mediator.Send(
                new CreateAccountCommand(
                    SelectedAccountVm.Name,
                    SelectedAccountVm.CurrentBalance,
                    SelectedAccountVm.Note,
                    SelectedAccountVm.IsExcluded));
    }
}