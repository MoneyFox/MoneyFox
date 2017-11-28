using MoneyFox.Business.ViewModels.Interfaces;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Windows.DesignTime
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

        public MvxAsyncCommand UpdateBalanceCommand => new MvxAsyncCommand(async () => { });
    }
}