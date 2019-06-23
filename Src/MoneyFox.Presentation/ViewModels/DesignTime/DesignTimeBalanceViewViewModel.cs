using GalaSoft.MvvmLight.Command;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.ViewModels.Interfaces;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeBalanceViewViewModel : IBalanceViewModel
    {
        /// <inheritdoc />
        public double TotalBalance => 1784;
        
        /// <inheritdoc />
        public double EndOfMonthBalance => 9784;

        /// <inheritdoc />
        public AsyncCommand UpdateBalanceCommand { get; } = null;
    }
}
