using System.Threading.Tasks;
using MoneyFox.Business.Manager;
using MoneyFox.Business.ViewModels.Interfaces;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public class BalanceViewModel : BaseViewModel, IBalanceViewModel
    {
        private readonly IBalanceCalculationManager balanceCalculationManager;

        private double totalBalance;
        private double endOfMonthBalance;

        public BalanceViewModel(IBalanceCalculationManager balanceCalculationManager)
        {
            this.balanceCalculationManager = balanceCalculationManager;
        }

        /// <summary>
        ///     Balance of all relevant accounts at the end of the month.
        /// </summary>
        public double TotalBalance
        {
            get { return totalBalance; }
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
            get { return endOfMonthBalance; }
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
        public MvxCommand UpdateBalanceCommand => new MvxCommand(UpdateBalance);

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        private async void UpdateBalance()
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