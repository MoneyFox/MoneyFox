namespace MoneyFox.ViewModels.Dashboard
{

    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Accounts;
    using AutoMapper;
    using Common.Extensions;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.ApplicationCore.Queries;
    using Core.Common.Interfaces;
    using Core.Common.Messages;
    using MediatR;
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    internal class DashboardViewModel : BaseViewModel
    {
        private readonly IMapper mapper;

        private readonly IMediator mediator;
        private readonly IDialogService dialogService;
        private ObservableCollection<AccountViewModel> accounts = new ObservableCollection<AccountViewModel>();
        private decimal assets;

        private ObservableCollection<DashboardBudgetEntryViewModel> budgetEntries = new ObservableCollection<DashboardBudgetEntryViewModel>();

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

        public AsyncRelayCommand GoToAddPaymentCommand
            => new AsyncRelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));

        public AsyncRelayCommand GoToAccountsCommand => new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.AccountListRoute));

        public AsyncRelayCommand GoToBudgetsCommand => new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.BudgetListRoute));

        public AsyncRelayCommand<AccountViewModel> GoToTransactionListCommand
            => new AsyncRelayCommand<AccountViewModel>(
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
