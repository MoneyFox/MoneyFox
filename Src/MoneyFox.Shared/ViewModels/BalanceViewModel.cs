using System.Linq;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Interfaces.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels
{
    public class BalanceViewModel : BaseViewModel, IBalanceViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IEndOfMonthManager endOfMonthManager;

        private double totalBalance;
        private double endOfMonthBalance;

        public BalanceViewModel(IAccountRepository accountRepository, IEndOfMonthManager endOfMonthManager)
        {
            this.accountRepository = accountRepository;
            this.endOfMonthManager = endOfMonthManager;
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
        private void UpdateBalance()
        {
            TotalBalance = GetTotalBalance();
            EndOfMonthBalance = GetEndOfMonthValue();
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected virtual double GetTotalBalance() => accountRepository.GetList()?.Sum(x => x.CurrentBalance) ?? 0;

        /// <summary>
        ///     Calculates the sum of all accounts at the end of the month.
        /// </summary>
        /// <returns>Sum of all balances including all payments to come till end of month.</returns>
        protected virtual double GetEndOfMonthValue()
        {
            return endOfMonthManager.GetTotalEndOfMonthBalance(accountRepository.GetList());
        }
    }
}