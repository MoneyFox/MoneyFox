using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Commands.CreateAccount;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AddAccountViewModel(IMediator mediator,
                                   IMapper mapper,
                                   IDialogService dialogService)
            : base(mediator, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        protected override async Task SaveAccountAsync()
        {
            await mediator.Send(new CreateAccountCommand(mapper.Map<Account>(SelectedAccountVm)));
        }
    }
}
