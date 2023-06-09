namespace MoneyFox.Ui.Views.Accounts.AccountList;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Accounts.DeleteAccountById;
using Core.Queries;
using MediatR;
using Resources.Strings;

public sealed class AccountListViewModel : BasePageViewModel, IRecipient<AccountsChangedMessage>
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;

    private ReadOnlyObservableCollection<AccountGroup> accountGroup = null!;

    private bool isRunning;

    public AccountListViewModel(IMediator mediator, IDialogService dialogService)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
    }

    public ReadOnlyObservableCollection<AccountGroup> AccountGroups
    {
        get => accountGroup;
        private set => SetProperty(field: ref accountGroup, newValue: value);
    }

    public AsyncRelayCommand GoToAddAccountCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddAccountRoute));

    public AsyncRelayCommand<AccountListItemViewModel> GoToEditAccountCommand
        => new(async avm => await Shell.Current.GoToAsync($"{Routes.EditAccountRoute}?accountId={avm.Id}"));

    public AsyncRelayCommand<AccountListItemViewModel> GoToTransactionListCommand
        => new(async avm => await Shell.Current.GoToAsync($"{Routes.PaymentListRoute}?accountId={avm.Id}"));

    public AsyncRelayCommand<AccountListItemViewModel> DeleteAccountCommand => new(DeleteAccountAsync);

    public void Receive(AccountsChangedMessage message)
    {
        InitializeAsync().GetAwaiter().GetResult();
    }

    public async Task InitializeAsync()
    {
        try
        {
            IsActive = true;
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            var accounts = await mediator.Send(new GetAccountsQuery());
            var accountListItems = accounts.Select(
                    a => new AccountListItemViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        CurrentBalance = a.CurrentBalance,
                        EndOfMonthBalance = mediator.Send(new GetAccountEndOfMonthBalanceQuery(a.Id)).GetAwaiter().GetResult(),
                        IsExcluded = a.IsExcluded
                    })
                .ToList();

            var accountGroups = accountListItems.GroupBy(a => a.IsExcluded).Select(g => new AccountGroup(isExcluded: g.Key, accountItems: g.ToList()));
            AccountGroups = new(new(accountGroups));
        }
        finally
        {
            isRunning = false;
        }
    }

    private async Task DeleteAccountAsync(AccountListItemViewModel? accountViewModel)
    {
        if (accountViewModel is null)
        {
            return;
        }

        if (await dialogService.ShowConfirmMessageAsync(
                title: Translations.DeleteTitle,
                message: Translations.DeleteAccountConfirmationMessage,
                positiveButtonText: Translations.YesLabel,
                negativeButtonText: Translations.NoLabel))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(accountViewModel.Id));
            await InitializeAsync();
        }
    }
}
