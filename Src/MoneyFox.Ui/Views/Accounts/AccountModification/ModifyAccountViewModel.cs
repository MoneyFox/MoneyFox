namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Queries;
using MediatR;
using Resources.Strings;

public abstract class ModifyAccountViewModel(IDialogService service, IMediator mediator, INavigationService navigationService) : BasePageViewModel
{
    private AccountViewModel selectedAccountVm = new();

    public virtual bool IsEdit => false;

    public virtual string Title => Translations.AddAccountTitle;

    protected IMediator Mediator { get; } = mediator;

    public AccountViewModel SelectedAccountVm
    {
        get => selectedAccountVm;

        set
        {
            selectedAccountVm = value;
            OnPropertyChanged();
        }
    }

    public AsyncRelayCommand SaveCommand => new(SaveAsync);

    protected abstract Task SaveAccountAsync();

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedAccountVm.Name))
        {
            await service.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.NameRequiredMessage);

            return;
        }

        try
        {
            var nameChanged = SelectedAccountVm.Id == 0
                              || !SelectedAccountVm.Name.Equals(await Mediator.Send(new GetAccountNameByIdQuery(SelectedAccountVm.Id)));

            var nameAlreadyTaken
                = await Mediator.Send(new GetIfAccountWithNameExistsQuery(accountName: SelectedAccountVm.Name, accountId: SelectedAccountVm.Id));

            if (nameChanged
                && nameAlreadyTaken
                && await service.ShowConfirmMessageAsync(title: Translations.DuplicatedNameTitle, message: Translations.DuplicateAccountMessage) is false)
            {
                return;
            }

            await service.ShowLoadingDialogAsync(Translations.SavingAccountMessage);
            await SaveAccountAsync();
            await navigationService.GoBack();
        }
        finally
        {
            await service.HideLoadingDialogAsync();
        }
    }
}
