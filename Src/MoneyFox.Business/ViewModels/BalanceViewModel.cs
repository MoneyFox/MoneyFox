using System.Threading.Tasks;
using MoneyFox.Business.Manager;
using MoneyFox.Business.ViewModels.Interfaces;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the BalanceView
    /// </summary>
    public class BalanceViewModel : BaseViewModel, IBalanceViewModel
    {
        private readonly IBalanceCalculationManager balanceCalculationManager;

        private double totalBalance;
        private double endOfMonthBalance;

        /// <summary>
        ///     Contstructor
        /// </summary>
        public BalanceViewModel(IBalanceCalculationManager balanceCalculationManager)
        {
            this.balanceCalculationManager = balanceCalculationManager;
        }

        /// <summary>
        ///     Balance of all relevant accounts at the end of the month.
        /// </summary>
        public double TotalBalance
        {
            get => totalBalance;
            set
            {
                totalBalance = value; 
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Current Balance of all accounts.
        /// </summary>
        public double EndOfMonthBalance
        {
            get => endOfMonthBalance;
            set
            {
                endOfMonthBalance = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        public MvxAsyncCommand UpdateBalanceCommand => new MvxAsyncCommand(UpdateBalance);

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        private async Task UpdateBalance()
        {
            TotalBalance = await GetTotalBalance();
            EndOfMonthBalance = await GetEndOfMonthValue();
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected virtual async Task<double> GetTotalBalance() => await balanceCalculationManager.GetTotalBalance();

        /// <summary>
        ///     Calculates the sum of all accounts at the end of the month.
        /// </summary>
        /// <returns>Sum of all balances including all payments to come till end of month.</returns>
        protected virtual async Task<double> GetEndOfMonthValue()
        {
            return await balanceCalculationManager.GetTotalEndOfMonthBalance();
        }
    }
}