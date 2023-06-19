namespace MoneyFox.Ui.Views.Ledgers.LedgerList;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Accounts.DeleteAccountById;
using Core.Queries;
using MediatR;
using Payments.PaymentList;
using Resources.Strings;

public sealed class LedgerListViewModel : BasePageViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    private bool isRunning;

    private ReadOnlyObservableCollection<LedgerGroup> ledgerGroups = null!;

    public LedgerListViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
    }

    public ReadOnlyObservableCollection<LedgerGroup> LedgerGroups
    {
        get => ledgerGroups;
        private set => SetProperty(field: ref ledgerGroups, newValue: value);
    }

    public AsyncRelayCommand GoToAddLedgerCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddAccountRoute));
    
    public AsyncRelayCommand<LedgerListItemViewModel> GoToTransactionListCommand
        => new(async avm => await navigationService.NavigateToAsync<PaymentListPage>(parameterName: "accountId", queryParameter: avm?.Id.ToString() ?? ""));

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
            LedgerGroups = new(new(accountGroups));
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
