using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Commands.CreateAccount;
using MoneyFox.Application.Accounts.Queries.GetIfAccountWithNameExists;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Accounts
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;

        public AddAccountViewModel(IMediator mediator,
                                   IMapper mapper,
                                   IDialogService dialogService)
            : base(dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
        }

        protected override async Task SaveAccountAsync()
        {
            if(await mediator.Send(new GetIfAccountWithNameExistsQuery(SelectedAccountVm.Name)))
            {
                await dialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateAccountMessage);
                return;
            }

            await mediator.Send(new CreateAccountCommand(mapper.Map<Account>(SelectedAccountVm)));
        }
    }
}
