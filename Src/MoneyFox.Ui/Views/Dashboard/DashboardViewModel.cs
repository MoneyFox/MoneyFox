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

public class DashboardViewModel : NavigableViewModel, IRecipient<BackupRestoredMessage>
{
    private ObservableCollection<AccountViewModel> accounts = new();
    private decimal assets;
    private decimal endOfMonthBalance;
    private decimal monthlyExpenses;
    private decimal monthlyIncomes;
    private readonly IMediator mediator1;
    private readonly IMapper mapper1;
    private readonly INavigationService service1;

    public DashboardViewModel(IMediator mediator, IMapper mapper, INavigationService service)
    {
        mediator1 = mediator;
        mapper1 = mapper;
        service1 = service;

        LoadData().GetAwaiter().GetResult();
    }

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

    public AsyncRelayCommand GoToAddPaymentCommand => new(() => service1.GoTo<AddPaymentViewModel>());

    public AsyncRelayCommand GoToAccountsCommand => new(() => service1.GoTo<AccountListViewModel>());

    public AsyncRelayCommand<AccountViewModel> GoToTransactionListCommand => new(accountViewModel => service1.GoTo<PaymentListViewModel>(accountViewModel!.Id));

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
        var accountVms = mapper1.Map<List<AccountViewModel>>(await mediator1.Send(new GetAccountsQuery())).OrderBy(avm => avm.IsExcluded).ThenBy(avm => avm.Name);
        Accounts = new(accountVms);
        foreach (var account in Accounts)
        {
            account.EndOfMonthBalance = await mediator1.Send(new GetAccountEndOfMonthBalanceQuery(account.Id));
        }

        Assets = await mediator1.Send(new GetIncludedAccountBalanceSummaryQuery());
        EndOfMonthBalance = await mediator1.Send(new GetTotalEndOfMonthBalanceQuery());
        MonthlyExpenses = await mediator1.Send(new GetMonthlyExpenseQuery());
        MonthlyIncomes = await mediator1.Send(new GetMonthlyIncomeQuery());
    }
}
