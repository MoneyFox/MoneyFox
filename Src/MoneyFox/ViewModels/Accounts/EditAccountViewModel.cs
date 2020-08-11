using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EditAccountViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }


        public async Task Init(int accountId)
        {
            SelectedAccountVm = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
        }
    }
}
