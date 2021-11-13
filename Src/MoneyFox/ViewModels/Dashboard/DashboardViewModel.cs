using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountEndOfMonthBalance;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary;
using MoneyFox.Application.Accounts.Queries.GetTotalEndOfMonthBalance;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Payments.Queries.GetMonthlyExpense;
using MoneyFox.Application.Payments.Queries.GetMonthlyIncome;
using MoneyFox.Extensions;
using MoneyFox.ViewModels.Accounts;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MoneyFox.ViewModels.Dashboard
{
    public class DashboardViewModel : ObservableRecipient
    {
        private decimal assets;
        private decimal endOfMonthBalance;
        private decimal monthlyIncomes;
        private decimal monthlyExpenses;
        private ObservableCollection<AccountViewModel> accounts = new ObservableCollection<AccountViewModel>();

        private ObservableCollection<DashboardBudgetEntryViewModel> budgetEntries =
            new ObservableCollection<DashboardBudgetEntryViewModel>();

        private bool isRunning;

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public DashboardViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        protected override void OnActivated()
            => Messenger.Register<DashboardViewModel, ReloadMessage>(this, (r, m) => r.InitializeAsync());

        protected override void OnDeactivated() => Messenger.Unregister<ReloadMessage>(this);

        public async Task InitializeAsync()
        {
            if(isRunning)
            {
                return;
            }

            try
            {
                isRunning = true;
                Accounts = mapper.Map<ObservableCollection<AccountViewModel>>(
                    await mediator.Send(new GetAccountsQuery()));
                Accounts.ForEach(
                    async x =>
                        x.EndOfMonthBalance = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(x.Id)));

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
                if(budgetEntries == value)
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
                if(accounts == value)
                {
                    return;
                }

                accounts = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand GoToAddPaymentCommand => new RelayCommand(
            async () =>
                await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));

        public RelayCommand GoToAccountsCommand =>
            new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.AccountListRoute));

        public RelayCommand GoToBudgetsCommand =>
            new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.BudgetListRoute));

        public RelayCommand<AccountViewModel> GoToTransactionListCommand
            => new RelayCommand<AccountViewModel>(
                async accountViewModel
                    => await Shell.Current.GoToAsync(
                        $"{ViewModelLocator.PaymentListRoute}?accountId={accountViewModel.Id}"));
    }
}