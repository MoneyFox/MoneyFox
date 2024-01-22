namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using Common.Navigation;
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
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    public EditAccountViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService) : base(
        dialogService: dialogService,
        mediator: mediator,
        navigationService: navigationService)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
        IsEdit = true;
    }

    public override string Title => string.Format(format: Translations.EditAccountTitle, arg0: SelectedAccountVm.Name);

    public AsyncRelayCommand DeleteCommand => new(DeleteAsync);

    public override async Task OnNavigatedAsync(object? parameter)
    {
        var accountId = Convert.ToInt32(parameter);
        var accountData = await mediator.Send(new GetAccountById.Query(accountId));
        SelectedAccountVm = new()
        {
            Id = accountData.AccountId,
            Name = accountData.Name,
            CurrentBalance = accountData.CurrentBalance.Amount,
            Note = accountData.Note,
            IsExcluded = accountData.IsExcluded,
            Created = accountData.Created,
            LastModified = accountData.LastModified
        };
    }

    protected override Task SaveAccountAsync()
    {
        var command = new UpdateAccount.Command(
            Id: SelectedAccountVm.Id,
            Name: SelectedAccountVm.Name,
            Note: SelectedAccountVm.Note,
            IsExcluded: SelectedAccountVm.IsExcluded);

        return mediator.Send(command);
    }

    private async Task DeleteAsync()
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeleteAccountConfirmationMessage))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(SelectedAccountVm.Id));
            await navigationService.GoBack();
        }
    }
}
