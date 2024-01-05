namespace MoneyFox.Ui.Views.Dashboard;

using System.Collections.ObjectModel;
using Accounts.AccountList;
using Accounts.AccountModification;
using AutoMapper;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using MediatR;
using Messages;
using Payments.PaymentList;
using Payments.PaymentModification;

public class DashboardViewModel(IMediator mediator, IMapper mapper, INavigationService navigationService) : NavigableViewModel,
    IRecipient<BackupRestoredMessage>
{
    private ObservableCollection<AccountViewModel> accounts = new();
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

    public ObservableCollection<AccountViewModel> Accounts
    {
        get => accounts;
        set => SetProperty(field: ref accounts, newValue: value);
    }

    public AsyncRelayCommand GoToAddPaymentCommand => new(() => navigationService.GoTo<AddPaymentViewModel>());

    public AsyncRelayCommand GoToAccountsCommand => new(() => navigationService.GoTo<AccountListViewModel>());

    public AsyncRelayCommand<AccountViewModel> GoToTransactionListCommand
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
        var accountVms = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery())).OrderBy(avm => avm.IsExcluded).ThenBy(avm => avm.Name);
        Accounts = new(accountVms);
        foreach (var account in Accounts)
        {
            account.EndOfMonthBalance = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(account.Id));
        }

        Assets = await mediator.Send(new GetIncludedAccountBalanceSummaryQuery());
        EndOfMonthBalance = await mediator.Send(new GetTotalEndOfMonthBalanceQuery());
        MonthlyExpenses = await mediator.Send(new GetMonthlyExpenseQuery());
        MonthlyIncomes = await mediator.Send(new GetMonthlyIncomeQuery());
    }
}
