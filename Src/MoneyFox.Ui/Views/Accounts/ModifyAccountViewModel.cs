namespace MoneyFox.Ui.Views.Accounts;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.Queries;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Interfaces;
using Core.Resources;
using MediatR;
using ViewModels;

internal abstract partial class ModifyAccountViewModel : BaseViewModel
{
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;

    private AccountViewModel selectedAccountVm = new();

    protected ModifyAccountViewModel(IDialogService dialogService, IMediator mediator, INavigationService navigationService)
    {
        this.dialogService = dialogService;
        this.navigationService = navigationService;
        Mediator = mediator;
    }

    public virtual bool IsEdit => false;

    public virtual string Title => Translations.AddAccountTitle;

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

    protected abstract Task SaveAccountAsync();

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedAccountVm.Name))
        {
            await dialogService.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.NameRequiredMessage);

            return;
        }

        var nameChanged = SelectedAccountVm.Id == 0 || !SelectedAccountVm.Name.Equals(await Mediator.Send(new GetAccountNameByIdQuery(SelectedAccountVm.Id)));
        if (nameChanged && await Mediator.Send(new GetIfAccountWithNameExistsQuery(accountName: SelectedAccountVm.Name, accountId: SelectedAccountVm.Id)))
        {
            if (!await dialogService.ShowConfirmMessageAsync(title: Translations.DuplicatedNameTitle, message: Translations.DuplicateAccountMessage))
            {
                return;
            }
        }

        await dialogService.ShowLoadingDialogAsync(Translations.SavingAccountMessage);
        await SaveAccountAsync();
        Messenger.Send(new ReloadMessage());
        await dialogService.HideLoadingDialogAsync();
        await navigationService.GoBackFromModalAsync();
    }
}
