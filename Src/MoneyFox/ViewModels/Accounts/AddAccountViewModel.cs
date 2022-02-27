namespace MoneyFox.ViewModels.Accounts
{
    using Core._Pending_.Common.Interfaces;
    using Core.Commands.Accounts.CreateAccount;
    using Core.Queries.Accounts.GetIfAccountWithNameExists;
    using Core.Resources;
    using MediatR;
    using System.Threading.Tasks;

    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly IDialogService dialogService;
        private readonly IMediator mediator;

        public AddAccountViewModel(IMediator mediator,
            IDialogService dialogService)
            : base(dialogService)
        {
            this.mediator = mediator;
            this.dialogService = dialogService;
        }

        protected override async Task SaveAccountAsync()
        {
            if(await mediator.Send(new GetIfAccountWithNameExistsQuery(SelectedAccountVm.Name)))
            {
                await dialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateAccountMessage);
                return;
            }

            await mediator.Send(
                new CreateAccountCommand(
                    SelectedAccountVm.Name,
                    SelectedAccountVm.CurrentBalance,
                    SelectedAccountVm.Note,
                    SelectedAccountVm.IsExcluded));
        }
    }
}