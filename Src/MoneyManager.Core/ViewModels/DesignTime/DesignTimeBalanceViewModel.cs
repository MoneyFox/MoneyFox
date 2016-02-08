using System;
using MoneyManager.Foundation.Interfaces.ViewModels;

namespace MoneyManager.Core.ViewModels.DesignTime
{
    public class DesignTimeBalanceViewModel : IBalanceViewModel
    {
        public DesignTimeBalanceViewModel()
        {
            TotalBalance = 12345;
            EndOfMonthBalance = 45677;
        }

        public bool IsPaymentView { get; set; }
        public double TotalBalance { get; set; }
        public double EndOfMonthBalance { get; set; }

        public void UpdateBalance(bool isPaymentView = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateBalance()
        {
        }
    }
}