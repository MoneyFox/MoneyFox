using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary;
using MoneyFox.Application.Accounts.Queries.GetTotalEndOfMonthBalance;
using MoneyFox.Application.Payments.Queries.GetMonthlyIncome;
using MoneyFox.Extensions;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Dashboard
{
    public class DashboardViewModel : ViewModelBase
    {
        private decimal assets;
        private decimal endOfMonthBalance;
        private decimal monthlyIncomes;
        private decimal monthlyExpenses;
        private ObservableCollection<AccountViewModel> accounts = new ObservableCollection<AccountViewModel>();
        private ObservableCollection<DashboardBudgetEntryViewModel> budgetEntries = new ObservableCollection<DashboardBudgetEntryViewModel>();

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public DashboardViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task InitializeAsync()
        {
            Accounts = mapper.Map<ObservableCollection<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));
            Assets = await mediator.Send(new GetIncludedAccountBalanceSummaryQuery());
            EndOfMonthBalance = await mediator.Send(new GetTotalEndOfMonthBalanceQuery());
            MonthlyExpenses = await mediator.Send(new GetMonthlyExpenseQuery());
            MonthlyIncomes = await mediator.Send(new GetMonthlyIncomeQuery());
        }

        public decimal Assets
        {
            get => assets;
            set
            {
                assets = value;
                RaisePropertyChanged();
            }
        }

        public decimal EndOfMonthBalance
        {
            get => endOfMonthBalance;
            set
            {
                endOfMonthBalance = value;
                RaisePropertyChanged();
            }
        }

        public decimal MonthlyIncomes
        {
            get => monthlyIncomes;
            set
            {
                monthlyIncomes = value;
                RaisePropertyChanged();
            }
        }

        public decimal MonthlyExpenses
        {
            get => monthlyExpenses;
            set
            {
                monthlyExpenses = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DashboardBudgetEntryViewModel> BudgetEntries
        {
            get => budgetEntries;
            private set
            {
                if(budgetEntries == value) return;
                budgetEntries = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<AccountViewModel> Accounts
        {
            get => accounts;
            private set
            {
                if(accounts == value) return;
                accounts = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GoToAddPaymentCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));
        public RelayCommand GoToAccountsCommand => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.AccountListRoute));
        public RelayCommand GoToBudgetsCommand => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.BudgetListRoute));

        public RelayCommand<AccountViewModel> GoToTransactionListCommand
            => new RelayCommand<AccountViewModel>(async (accountViewModel)
                => await Shell.Current.GoToAsync($"{ViewModelLocator.PaymentListRoute}?accountId={accountViewModel.Id}"));
    }
}
