namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Queries;
using MoneyFox.Ui.Messages;
using MoneyFox.Ui.Resources.Strings;

// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class ModifyAccountViewModel : BasePageViewModel
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
        var nameAlreadyTaken = await Mediator.Send(new GetIfAccountWithNameExistsQuery(accountName: SelectedAccountVm.Name, accountId: SelectedAccountVm.Id));
        if (nameChanged
            && nameAlreadyTaken
            && await dialogService.ShowConfirmMessageAsync(title: Translations.DuplicatedNameTitle, message: Translations.DuplicateAccountMessage) is false)
        {
            return;
        }

        await dialogService.ShowLoadingDialogAsync(Translations.SavingAccountMessage);
        await SaveAccountAsync();
        Messenger.Send(new ReloadMessage());
        await dialogService.HideLoadingDialogAsync();
        await navigationService.GoBackFromModalAsync();
    }
}
