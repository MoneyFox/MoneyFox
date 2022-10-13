using MoneyFox.Ui.ViewModels.Accounts;

namespace MoneyFox.Ui.Views.Accounts;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.Commands.Accounts.DeleteAccountById;
using MoneyFox.Core.Commands.Accounts.UpdateAccount;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Resources;

internal partial class EditAccountViewModel : ModifyAccountViewModel
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;

    public EditAccountViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, INavigationService navigationService)
        : base(dialogService: dialogService, mediator: mediator, navigationService: navigationService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
    }

    public override bool IsEdit => true;
    public override string Title => Strings.EditAccountTitle;

    public async Task InitializeAsync(int accountId)
    {
        SelectedAccountVm = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
    }

    protected override async Task SaveAccountAsync()
    {
        await mediator.Send(new UpdateAccountCommand(mapper.Map<Account>(SelectedAccountVm)));
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(SelectedAccountVm.Id));
            await navigationService.GoBackFromModalAsync();
        }
    }
}
