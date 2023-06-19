namespace MoneyFox.Ui.Views.Ledgers.LedgerList;

using System.Collections.ObjectModel;
using Accounts;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Features._Legacy_.Accounts.DeleteAccountById;
using MoneyFox.Core.Queries;
using MoneyFox.Ui.Resources.Strings;

public sealed class LedgerListViewModel : BasePageViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;

    private ReadOnlyObservableCollection<LedgerGroup> ledgerGroup = null!;

    private bool isRunning;

    public LedgerListViewModel(IMediator mediator, IDialogService dialogService)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
    }

    public ReadOnlyObservableCollection<LedgerGroup> LedgerGroup
    {
        get => ledgerGroup;
        private set => SetProperty(field: ref ledgerGroup, newValue: value);
    }

    public AsyncRelayCommand GoToAddLedgerCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddAccountRoute));

    public AsyncRelayCommand<LedgerListItemViewModel> GoToEditAccountCommand
        => new(async avm => await Shell.Current.GoToAsync($"{Routes.EditAccountRoute}?accountId={avm.Id}"));

    public AsyncRelayCommand<LedgerListItemViewModel> GoToTransactionListCommand
        => new(async avm => await Shell.Current.GoToAsync($"{Routes.PaymentListRoute}?accountId={avm.Id}"));

    public AsyncRelayCommand<LedgerListItemViewModel> DeleteAccountCommand => new(DeleteAccountAsync);

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
                    a => new LedgerListItemViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        CurrentBalance = a.CurrentBalance,
                        EndOfMonthBalance = mediator.Send(new GetAccountEndOfMonthBalanceQuery(a.Id)).GetAwaiter().GetResult(),
                        IsExcluded = a.IsExcluded
                    })
                .ToList();

            var accountGroups = accountListItems.GroupBy(a => a.IsExcluded).Select(g => new LedgerGroup(isExcluded: g.Key, accountItems: g.ToList()));
            LedgerGroup = new(new(accountGroups));
        }
        finally
        {
            isRunning = false;
        }
    }

    private async Task DeleteAccountAsync(LedgerListItemViewModel? accountViewModel)
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
