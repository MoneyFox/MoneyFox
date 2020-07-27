using MoneyFox.Presentation.ViewModels.Interfaces;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeBalanceViewViewModel : IBalanceViewModel
    {
        /// <inheritdoc/>
        public decimal TotalBalance => 1784;

        /// <inheritdoc/>
        public decimal EndOfMonthBalance => 9784;

        /// <inheritdoc/>
        public AsyncCommand UpdateBalanceCommand { get; }
    }
}
