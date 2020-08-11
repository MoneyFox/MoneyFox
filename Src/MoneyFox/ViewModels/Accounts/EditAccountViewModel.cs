using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Commands.UpdateAccount;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Domain.Entities;
using MoneyFox.Services;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EditAccountViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IDialogService dialogService)
            : base(mediator, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }


        public async Task Init(int accountId)
        {
            SelectedAccountVm = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
        }

        protected override async Task SaveAccountAsync()
        {
            await mediator.Send(new UpdateAccountCommand(mapper.Map<Account>(SelectedAccountVm)));
        }
    }
}
