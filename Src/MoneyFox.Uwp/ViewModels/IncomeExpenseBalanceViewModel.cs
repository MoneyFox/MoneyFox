using GalaSoft.MvvmLight;

namespace MoneyFox.Uwp.ViewModels
{
    public class IncomeExpenseBalanceViewModel : ViewModelBase
    {
        private decimal totalEarned;
        private decimal totalSpent;

        public decimal TotalEarned
        {
            get => totalEarned;
            set
            {
                if (totalEarned == value) return;
                totalEarned = value;
                RaisePropertyChanged();
            }
        }

        public decimal TotalSpent
        {
            get => totalSpent;
            set
            {
                if (totalSpent == value) return;
                totalSpent = value;
                RaisePropertyChanged();
            }
        }
    }
}
