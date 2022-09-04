namespace MoneyFox.Ui.ViewModels.Accounts;

using AutoMapper;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.Commands.Accounts.UpdateAccount;
using MoneyFox.Core.Common.Interfaces;

internal class EditAccountViewModel : ModifyAccountViewModel
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
