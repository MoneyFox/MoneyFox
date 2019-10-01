using System;

namespace MoneyFox.Presentation.ViewModels
{
    public class IncomeExpenseBalanceViewModel : BaseViewModel
    {
        private double totalIncome;
        private double totalSpent;

        public double TotalIncome
        {
            get => totalIncome;
            set
            {
                if(Math.Abs(totalIncome - value) < 0.01) return;
                totalIncome = value;
                RaisePropertyChanged();
            }
        }

        public double TotalSpent
        {
            get => totalSpent;
            set
            {
                if (Math.Abs(totalSpent - value) < 0.01) return;
                totalSpent = value;
                RaisePropertyChanged();
            }
        }
    }
}
