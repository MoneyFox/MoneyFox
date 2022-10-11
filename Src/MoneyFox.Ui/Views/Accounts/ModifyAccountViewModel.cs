namespace MoneyFox.Ui.ViewModels.Accounts;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Core.Resources;

internal abstract class ModifyAccountViewModel : BaseViewModel
{
    private readonly IDialogService dialogService;

    private AccountViewModel selectedAccountVm = new();

    protected ModifyAccountViewModel(IDialogService dialogService, IMediator mediator)
    {
        this.dialogService = dialogService;
        Mediator = mediator;
    }

    public virtual bool IsEdit => false;

    public virtual string Title => Strings.AddAccountTitle;

    protected IMediator Mediator { get; }

    /// <summary>
    ///     The currently selected CategoryViewModel
    /// </summary>
    public AccountViewModel SelectedAccountVm
    {
        get => selectedAccountVm;

        set
        {
            selectedAccountVm = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand SaveCommand => new(async () => await SaveAccountBaseAsync());

    protected abstract Task SaveAccountAsync();

    private async Task SaveAccountBaseAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedAccountVm.Name))
        {
            await dialogService.ShowMessageAsync(title: Strings.MandatoryFieldEmptyTitle, message: Strings.NameRequiredMessage);

            return;
        }

        var nameChanged = SelectedAccountVm.Id == 0 || !SelectedAccountVm.Name.Equals(await Mediator.Send(new GetAccountNameByIdQuery(SelectedAccountVm.Id)));
        if (nameChanged && await Mediator.Send(new GetIfAccountWithNameExistsQuery(accountName: SelectedAccountVm.Name, accountId: SelectedAccountVm.Id)))
        {
            if (!await dialogService.ShowConfirmMessageAsync(title: Strings.DuplicatedNameTitle, message: Strings.DuplicateAccountMessage))
            {
                return;
            }
        }

        await dialogService.ShowLoadingDialogAsync(Strings.SavingAccountMessage);
        await SaveAccountAsync();
        Messenger.Send(new ReloadMessage());
        await dialogService.HideLoadingDialogAsync();
        await Application.Current.MainPage.Navigation.PopModalAsync();
    }
}
