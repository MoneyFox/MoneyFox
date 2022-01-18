using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Commands.Accounts.CreateAccount;
using MoneyFox.Core.Queries.Accounts.GetIfAccountWithNameExists;
using MoneyFox.Core.Resources;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
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