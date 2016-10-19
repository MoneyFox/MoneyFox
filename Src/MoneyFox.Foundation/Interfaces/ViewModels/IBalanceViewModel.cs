using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.Interfaces.ViewModels
{
    public interface IBalanceViewModel
    {
        /// <summary>
        ///     Balance of either the selected account or all relevant accounts at the end of the month.
        /// </summary>
        double TotalBalance { get; set; }

        /// <summary>
        ///     Current Balance of either the selected account or all accounts.
        /// </summary>
        double EndOfMonthBalance { get; set; }

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        MvxCommand UpdateBalanceCommand { get; }
    }
}