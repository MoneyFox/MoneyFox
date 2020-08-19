using AutoMapper;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary;
using MoneyFox.Application.Accounts.Queries.GetTotalEndOfMonthBalance;
using MoneyFox.Extensions;
using MoneyFox.ViewModels.Accounts;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Dashboard
{
    public class DashboardViewModel : BaseViewModel
    {
        private decimal assets;
        private ObservableCollection<AccountViewModel> accounts = new ObservableCollection<AccountViewModel>();

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
        }

        private decimal endOfMonthBalance = 13000;
        private decimal monthlyIncomes = 7000;
        private decimal monthlyExpenses = 5000;


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

        public ObservableCollection<DashboardBudgetEntryViewModel> BudgetEntries { get; set; } = new ObservableCollection<DashboardBudgetEntryViewModel>
        {
            new DashboardBudgetEntryViewModel{ BudgetName = "Food", Progress = 0.7},
            new DashboardBudgetEntryViewModel{ BudgetName = "Drinks", Progress = 0.5},
            new DashboardBudgetEntryViewModel{ BudgetName = "Books", Progress = 0.2}
        };

        public ObservableCollection<AccountViewModel> Accounts
        {
            get => accounts;
            set
            {
                if(accounts == value) return;
                accounts = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GoToAddPaymentCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));
        public RelayCommand GoToAccountsCommand => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.AccountListRoute));
        public RelayCommand GoToBudgetsCommand => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.BudgetListRoute));
    }
}
