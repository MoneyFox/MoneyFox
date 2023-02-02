namespace MoneyFox.Ui.Views.Dashboard;

using System.Collections.ObjectModel;
using Accounts;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Messages;
using Core.Queries;
using MediatR;

internal class DashboardViewModel : BasePageViewModel, IRecipient<ReloadMessage>
{
    private readonly IMapper mapper;

    private readonly IMediator mediator;
    private ObservableCollection<AccountViewModel> accounts = new();
    private decimal assets;

    private ObservableCollection<DashboardBudgetEntryViewModel> budgetEntries = new();

    private decimal endOfMonthBalance;

    private bool isRunning;
    private decimal monthlyExpenses;
    private decimal monthlyIncomes;

    public DashboardViewModel(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public decimal Assets
    {
        get => assets;

        set
        {
            assets = value;
            OnPropertyChanged();
        }
    }

    public decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;

        set
        {
            endOfMonthBalance = value;
            OnPropertyChanged();
        }
    }

    public decimal MonthlyIncomes
    {
        get => monthlyIncomes;

        set
        {
            monthlyIncomes = value;
            OnPropertyChanged();
        }
    }

    public decimal MonthlyExpenses
    {
        get => monthlyExpenses;

        set
        {
            monthlyExpenses = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<DashboardBudgetEntryViewModel> BudgetEntries
    {
        get => budgetEntries;

        private set
        {
            if (budgetEntries == value)
            {
                return;
            }

            budgetEntries = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<AccountViewModel> Accounts
    {
        get => accounts;

        private set
        {
            if (accounts == value)
            {
                return;
            }

            accounts = value;
            OnPropertyChanged();
        }
    }

    public AsyncRelayCommand GoToAddPaymentCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddPaymentRoute));

    public AsyncRelayCommand GoToAccountsCommand => new(async () => await Shell.Current.GoToAsync(Routes.AccountListRoute));

    public AsyncRelayCommand GoToBudgetsCommand => new(async () => await Shell.Current.GoToAsync(Routes.BudgetListRoute));

    public AsyncRelayCommand<AccountViewModel> GoToTransactionListCommand
        => new(async accountViewModel => await Shell.Current.GoToAsync($"{Routes.PaymentListRoute}?accountId={accountViewModel!.Id}"));

    public async void Receive(ReloadMessage message)
    {
        await InitializeAsync();
    }

    public async Task InitializeAsync()
    {
        if (isRunning)
        {
            return;
        }

        try
        {
            isRunning = true;
            Accounts = mapper.Map<ObservableCollection<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));
            foreach (var account in Accounts)
            {
                account.EndOfMonthBalance = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(account.Id));
            }

            Assets = await mediator.Send(new GetIncludedAccountBalanceSummaryQuery());
            EndOfMonthBalance = await mediator.Send(new GetTotalEndOfMonthBalanceQuery());
            MonthlyExpenses = await mediator.Send(new GetMonthlyExpenseQuery());
            MonthlyIncomes = await mediator.Send(new GetMonthlyIncomeQuery());
        }
        finally
        {
            isRunning = false;
        }
    }
}
