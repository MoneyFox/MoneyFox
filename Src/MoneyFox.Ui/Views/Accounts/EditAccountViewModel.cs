namespace MoneyFox.Ui.Views.Accounts;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Accounts.DeleteAccountById;
using Core.Features._Legacy_.Accounts.UpdateAccount;
using Core.Interfaces;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Resources.Strings;

// ReSharper disable once PartialTypeWithSinglePart
public partial class EditAccountViewModel : ModifyAccountViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    public EditAccountViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, INavigationService navigationService) : base(
        dialogService: dialogService,
        mediator: mediator,
        navigationService: navigationService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
    }

    public override bool IsEdit => true;
    public override string Title => string.Format(format: Translations.EditAccountTitle, arg0: SelectedAccountVm.Name);

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
        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeleteAccountConfirmationMessage))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(SelectedAccountVm.Id));
            await navigationService.GoBackFromModalAsync();
        }
    }
}
