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
    }
}
