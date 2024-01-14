namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Queries;
using MediatR;
using Resources.Strings;

public abstract class ModifyAccountViewModel(IDialogService dialogService, ISender mediator, INavigationService navigationService) : NavigableViewModel
{
    private AccountViewModel selectedAccountVm = new();

    public bool IsEdit { get; protected init; }

    public abstract string Title { get; }

    public AccountViewModel SelectedAccountVm
    {
        get => selectedAccountVm;
        protected set => SetProperty(field: ref selectedAccountVm, newValue: value);
    }

    public AsyncRelayCommand SaveCommand => new(execute: SaveAsync, canExecute: () => SelectedAccountVm.IsValid);

    protected abstract Task SaveAccountAsync();

    private async Task SaveAsync()
    {
        try
        {
            var nameChanged = SelectedAccountVm.Id == 0
                              || !SelectedAccountVm.Name.Equals(await mediator.Send(new GetAccountNameByIdQuery(SelectedAccountVm.Id)));

            var nameAlreadyTaken
                = await mediator.Send(new GetIfAccountWithNameExistsQuery(accountName: SelectedAccountVm.Name, accountId: SelectedAccountVm.Id));

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
