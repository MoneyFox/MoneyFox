using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Commands.Accounts.CreateAccount;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly IDialogService dialogService;
        private readonly IMediator mediator;

        public AddAccountViewModel(IMediator mediator,
            IDialogService dialogService)
            : base(dialogService, mediator)
        {
            this.mediator = mediator;
            this.dialogService = dialogService;
        }

        protected override async Task SaveAccountAsync()
        {
            await mediator.Send(
                new CreateAccountCommand(
                    SelectedAccountVm.Name,
                    SelectedAccountVm.CurrentBalance,
                    SelectedAccountVm.Note,
                    SelectedAccountVm.IsExcluded));
        }
    }
}