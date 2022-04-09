namespace MoneyFox.Win.ViewModels.Interfaces;

using CommunityToolkit.Mvvm.Input;

public interface IBalanceViewModel
{
    /// <summary>
    ///     Balance of either the selected account or all relevant accounts at the end of the month.
    /// </summary>
    decimal TotalBalance { get; }

    /// <summary>
    ///     Current Balance of either the selected account or all accounts.
    /// </summary>
    decimal EndOfMonthBalance { get; }

    /// <summary>
    ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
    ///     itself and show different data.
    /// </summary>
    AsyncRelayCommand UpdateBalanceCommand { get; }
}
