namespace MoneyFox.Ui.Views.Dashboard;

using System.Collections.ObjectModel;
using Accounts.AccountList;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using Domain;
using MediatR;
using Messages;
using Payments.PaymentList;
using Payments.PaymentModification;

public class DashboardViewModel(IMediator mediator, INavigationService navigationService) : NavigableViewModel, IRecipient<BackupRestoredMessage>
{
    private decimal assets;
    private decimal endOfMonthBalance;
    private decimal monthlyExpenses;
    private decimal monthlyIncomes;

    public decimal Assets
    {
        get => assets;
        set => SetProperty(field: ref assets, newValue: value);
    }

    public decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;
        set => SetProperty(field: ref endOfMonthBalance, newValue: value);
    }

    public decimal MonthlyIncomes
    {
        get => monthlyIncomes;
        set => SetProperty(field: ref monthlyIncomes, newValue: value);
    }

    public decimal MonthlyExpenses
    {
        get => monthlyExpenses;
        set => SetProperty(field: ref monthlyExpenses, newValue: value);
    }

    public ObservableCollection<DashboardAccountViewModel> Accounts { get; } = new();

    public AsyncRelayCommand GoToAddPaymentCommand => new(() => navigationService.GoTo<AddPaymentViewModel>());

    public AsyncRelayCommand GoToAccountsCommand => new(() => navigationService.GoTo<AccountListViewModel>());

    public AsyncRelayCommand<DashboardAccountViewModel> GoToTransactionListCommand
        => new(accountViewModel => navigationService.GoTo<PaymentListViewModel>(accountViewModel!.Id));

    public void Receive(BackupRestoredMessage message)
    {
        LoadData().GetAwaiter().GetResult();
    }

    public override Task OnNavigatedAsync(object? parameter)
    {
        return LoadData();
    }

    private async Task LoadData()
    {
        var accountData = await mediator.Send(new GetAccountsQuery());
        var orderedAccounts = accountData.OrderBy(avm => avm.IsExcluded).ThenBy(avm => avm.Name);
        foreach (var account in orderedAccounts)
        {
            var endOfMonth = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(account.Id));
            Accounts.Add(
                new(
                    Id: account.Id,
                    Name: account.Name,
                    CurrentBalance: account.CurrentBalance,
                    EndOfMonthBalance: new(amount: endOfMonth, currency: account.CurrentBalance.Currency)));
        }

        Assets = await mediator.Send(new GetIncludedAccountBalanceSummaryQuery());
        EndOfMonthBalance = await mediator.Send(new GetTotalEndOfMonthBalanceQuery());
        MonthlyExpenses = await mediator.Send(new GetMonthlyExpenseQuery());
        MonthlyIncomes = await mediator.Send(new GetMonthlyIncomeQuery());
    }
}

public record DashboardAccountViewModel(int Id, string Name, Money CurrentBalance, Money EndOfMonthBalance);
