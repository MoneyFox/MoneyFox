using GalaSoft.MvvmLight.Command;
using MoneyFox.Extensions;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Dashboard
{
    public class DashboardViewModel : BaseViewModel
    {
        private decimal assets = 13000;
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

        public RelayCommand GoToAddPaymentCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));
        public RelayCommand GoToAccountsCommand => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.AccountListRoute));
        public RelayCommand GoToBudgetsCommand => new RelayCommand(async () => await Shell.Current.GoToAsync(ViewModelLocator.BudgetListRoute));
    }
}
