namespace MoneyFox.Ui.Views.Accounts.AccountList;

using System.Collections.ObjectModel;
using AccountModification;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Accounts.DeleteAccountById;
using Core.Queries;
using MediatR;
using Payments.PaymentList;
using Resources.Strings;

public sealed class AccountListViewModel(ISender mediator, IDialogService service, INavigationService navigationService) : NavigableViewModel
{
    private ReadOnlyObservableCollection<AccountGroup> accountGroup = null!;

    public ReadOnlyObservableCollection<AccountGroup> AccountGroups
    {
        get => accountGroup;
        private set => SetProperty(field: ref accountGroup, newValue: value);
    }

    public AsyncRelayCommand GoToAddAccountCommand => new(() => navigationService.GoTo<AddAccountViewModel>());

    public AsyncRelayCommand<AccountListItemViewModel> GoToEditAccountCommand => new(avm => navigationService.GoTo<EditAccountViewModel>(avm!.Id));

    public AsyncRelayCommand<AccountListItemViewModel> GoToTransactionListCommand => new(avm => navigationService.GoTo<PaymentListViewModel>(avm!.Id));

    public AsyncRelayCommand<AccountListItemViewModel> DeleteAccountCommand => new(DeleteAccountAsync);

    public override Task OnNavigatedAsync(object? parameter)
    {
        return LoadAccountList();
    }

    public override Task OnNavigatedBackAsync(object? parameter)
    {
        return LoadAccountList();
    }

    private async Task LoadAccountList()
    {
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

    private async Task DeleteAccountAsync(AccountListItemViewModel accountViewModel)
    {
        if (await service.ShowConfirmMessageAsync(
                title: Translations.DeleteTitle,
                message: Translations.DeleteAccountConfirmationMessage,
                positiveButtonText: Translations.YesLabel,
                negativeButtonText: Translations.NoLabel))
        {
            await mediator.Send(new DeactivateAccountByIdCommand(accountViewModel.Id));
            await LoadAccountList();
        }
    }
}
