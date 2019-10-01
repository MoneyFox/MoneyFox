using System;

namespace MoneyFox.Presentation.ViewModels
{
    public class IncomeExpenseBalanceViewModel : BaseViewModel
    {
        private decimal totalIncome;
        private decimal totalSpent;

        public decimal TotalIncome
        {
            get => totalIncome;
            set
            {
                if(totalIncome == value) return;
                totalIncome = value;
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
