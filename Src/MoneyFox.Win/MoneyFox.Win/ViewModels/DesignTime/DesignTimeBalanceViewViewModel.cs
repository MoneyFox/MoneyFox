using CommunityToolkit.Mvvm.Input;
using MoneyFox.Win.ViewModels.Interfaces;

namespace MoneyFox.Win.ViewModels.DesignTime
{
    public class DesignTimeBalanceViewViewModel : IBalanceViewModel
    {
        /// <inheritdoc />
        public decimal TotalBalance => 1784;

        /// <inheritdoc />
        public decimal EndOfMonthBalance => 9784;

        /// <inheritdoc />
        public AsyncRelayCommand UpdateBalanceCommand { get; } = null!;
    }
}