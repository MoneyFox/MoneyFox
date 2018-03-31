using MoneyFox.Business.ViewModels.Interfaces;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeBalanceViewViewModel : IBalanceViewModel
    {
        /// <inheritdoc />
        public double TotalBalance => 1784;
        
        /// <inheritdoc />
        public double EndOfMonthBalance => 9784;

        /// <inheritdoc />
        public MvxAsyncCommand UpdateBalanceCommand { get; } = null;
    }
}
