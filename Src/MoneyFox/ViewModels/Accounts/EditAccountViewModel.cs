namespace MoneyFox.ViewModels.Accounts
{

    using System.Threading.Tasks;
    using AutoMapper;
    using Core.Aggregates;
    using Core.Commands.Accounts.UpdateAccount;
    using Core.Common.Interfaces;
    using Core.Queries;
    using MediatR;

    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public EditAccountViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService) : base(dialogService: dialogService, mediator: mediator)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public override bool IsEdit => true;

        public async Task InitializeAsync(int accountId)
        {
            SelectedAccountVm = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
        }

        protected override async Task SaveAccountAsync()
        {
            await mediator.Send(new UpdateAccountCommand(mapper.Map<Account>(SelectedAccountVm)));
        }
    }

}
