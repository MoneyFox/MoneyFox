using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.Interfaces.ViewModels;

namespace MoneyFox.Core.ViewModels.DesignTime
{
    public class DesignTimeBalanceViewModel : IBalanceViewModel
    {
        public DesignTimeBalanceViewModel()
        {
            TotalBalance = 12345;
            EndOfMonthBalance = 45677;
        }

        public bool IsPaymentViewModelView { get; set; }
        public double TotalBalance { get; set; }

        public double EndOfMonthBalance { get; set; }

        public RelayCommand UpdateBalanceCommand => new RelayCommand(() => { });
    }
}