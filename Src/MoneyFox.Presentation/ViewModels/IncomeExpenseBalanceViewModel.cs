namespace MoneyFox.Presentation.ViewModels
{
    public class IncomeExpenseBalanceViewModel : BaseViewModel
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
