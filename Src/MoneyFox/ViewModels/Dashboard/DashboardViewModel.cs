namespace MoneyFox.ViewModels.Dashboard
{

    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Accounts;
    using AutoMapper;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Messages;
    using Core.ApplicationCore.Queries;
    using Extensions;
    using MediatR;
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    public class DashboardViewModel : ObservableRecipient
    {
        private readonly IMapper mapper;

        private readonly IMediator mediator;
        private ObservableCollection<AccountViewModel> accounts = new ObservableCollection<AccountViewModel>();
        private decimal assets;

        private ObservableCollection<DashboardBudgetEntryViewModel> budgetEntries = new ObservableCollection<DashboardBudgetEntryViewModel>();

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

        public RelayCommand GoToAddPaymentCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));

        public RelayCommand GoToAccountsCommand => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.AccountListRoute));

        public RelayCommand GoToBudgetsCommand => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.BudgetListRoute));

        public RelayCommand<AccountViewModel> GoToTransactionListCommand
            => new RelayCommand<AccountViewModel>(
                async accountViewModel => await Shell.Current.GoToAsync($"{ViewModelLocator.PaymentListRoute}?accountId={accountViewModel.Id}"));

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
                Accounts.ForEach(async x => x.EndOfMonthBalance = await mediator.Send(new GetAccountEndOfMonthBalanceQuery(x.Id)));
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

}
