namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Accounts.DeleteAccountById;
using Core.Features._Legacy_.Accounts.UpdateAccount;
using Core.Queries;
using MediatR;
using Resources.Strings;

public class EditAccountViewModel : ModifyAccountViewModel
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

    public AsyncRelayCommand DeleteCommand => new(DeleteAsync);

    public async Task InitializeAsync(int accountId)
    {
        SelectedAccountVm = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));
    }

    protected override async Task SaveAccountAsync()
    {
        // Due to a bug in .net maui, the loading dialog can only be called after any other dialog
        await dialogService.ShowLoadingDialogAsync(Translations.SavingAccountMessage);
        var command = new UpdateAccount.Command(
            Id: SelectedAccountVm.Id,
            Name: SelectedAccountVm.Name,
            Note: SelectedAccountVm.Note,
            IsExcluded: SelectedAccountVm.IsExcluded);

        await mediator.Send(command);
    }

    private async Task DeleteAsync()
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeleteAccountConfirmationMessage))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(SelectedAccountVm.Id));
            await navigationService.GoBackFromModalAsync();
        }
    }
}
