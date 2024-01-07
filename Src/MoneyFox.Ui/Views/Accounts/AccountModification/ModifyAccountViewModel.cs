namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Queries;
using MediatR;
using Resources.Strings;

public abstract class ModifyAccountViewModel(IDialogService dialogService, IMediator mediator, INavigationService navigationService) : NavigableViewModel
{
    private AccountViewModelNew selectedAccountVm = new();

    public virtual bool IsEdit => false;

    public virtual string Title => Translations.AddAccountTitle;

    protected IMediator Mediator { get; } = mediator;

    public AccountViewModelNew SelectedAccountVm
    {
        get => selectedAccountVm;
        protected set => SetProperty(field: ref selectedAccountVm, newValue: value);
    }

    public AsyncRelayCommand SaveCommand => new(SaveAsync);

    protected abstract Task SaveAccountAsync();

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedAccountVm.Name))
        {
            await dialogService.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.NameRequiredMessage);

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
                && await dialogService.ShowConfirmMessageAsync(title: Translations.DuplicatedNameTitle, message: Translations.DuplicateAccountMessage) is false)
            {
                return;
            }

            await dialogService.ShowLoadingDialogAsync(Translations.SavingAccountMessage);
            await SaveAccountAsync();
            await navigationService.GoBack();
        }
        finally
        {
            await dialogService.HideLoadingDialogAsync();
        }
    }
}
