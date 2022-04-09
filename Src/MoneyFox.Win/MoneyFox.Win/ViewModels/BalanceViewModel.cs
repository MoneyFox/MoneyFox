namespace MoneyFox.Win.ViewModels;

using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Interfaces;
using Microsoft.AppCenter.Crashes;

/// <summary>
///     Representation of the BalanceView
/// </summary>
public class BalanceViewModel : ObservableObject, IBalanceViewModel
{
    private readonly IBalanceCalculationService balanceCalculationService;

    private decimal totalBalance;
    private decimal endOfMonthBalance;

    public BalanceViewModel(IBalanceCalculationService balanceCalculationService)
    {
        this.balanceCalculationService = balanceCalculationService;
    }

    /// <summary>
    ///     Balance of all relevant accounts at the end of the month.
    /// </summary>
    public decimal TotalBalance
    {
        get => totalBalance;

        set
        {
            totalBalance = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Current Balance of all accounts.
    /// </summary>
    public decimal EndOfMonthBalance
    {
        get => endOfMonthBalance;

        set
        {
            endOfMonthBalance = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
    ///     itself and show different data.
    /// </summary>
    public AsyncRelayCommand UpdateBalanceCommand => new(UpdateBalanceAsync);

    /// <summary>
    ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
    ///     itself and show different data.
    /// </summary>
    private async Task UpdateBalanceAsync()
    {
        try
        {
            TotalBalance = await CalculateTotalBalanceAsync();
            EndOfMonthBalance = await GetEndOfMonthValueAsync();
        }
        catch (Exception ex)
        {
            Crashes.TrackError(ex);
        }
    }

    /// <summary>
    ///     Calculates the sum of all accounts at the current moment.
    /// </summary>
    /// <returns>Sum of the balance of all accounts.</returns>
    protected virtual async Task<decimal> CalculateTotalBalanceAsync()
    {
        return await balanceCalculationService.GetTotalBalanceAsync();
    }

    /// <summary>
    ///     Calculates the sum of all accounts at the end of the month.
    /// </summary>
    /// <returns>Sum of all balances including all payments to come till end of month.</returns>
    protected virtual async Task<decimal> GetEndOfMonthValueAsync()
    {
        return await balanceCalculationService.GetTotalEndOfMonthBalanceAsync();
    }
}
