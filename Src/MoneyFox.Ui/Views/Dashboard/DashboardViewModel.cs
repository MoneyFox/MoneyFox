namespace MoneyFox.Ui.Views.Dashboard;

using System.Collections.ObjectModel;
using AutoMapper;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Ui;
using MoneyFox.Ui.ViewModels;
using MoneyFox.Ui.ViewModels.Accounts;

internal class DashboardViewModel : BaseViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;

    private readonly IMediator mediator;
    private ObservableCollection<AccountViewModel> accounts = new();
    private decimal assets;

    private ObservableCollection<DashboardBudgetEntryViewModel> budgetEntries = new();

    private decimal endOfMonthBalance;

    private bool isRunning;
    private decimal monthlyExpenses;
    private decimal monthlyIncomes;

    public DashboardViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
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
        => new(async accountViewModel => await Shell.Current.GoToAsync($"{Routes.PaymentListRoute}?accountId={accountViewModel.Id}"));

    protected override void OnActivated()
    {
        Messenger.Register<DashboardViewModel, ReloadMessage>(recipient: this, handler: (r, m) => r.InitializeAsync());
    }

    protected override void OnDeactivated()
    {
        Messenger.Unregister<ReloadMessage>(this);
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
            foreach (AccountViewModel account in Accounts)
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

        IsActive = true;
    }
}
