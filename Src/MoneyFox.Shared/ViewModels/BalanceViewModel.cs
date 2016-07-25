using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.ViewModels;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class BalanceViewModel : BaseViewModel, IBalanceViewModel
    {
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<Payment> paymentRepository;

        public BalanceViewModel(IRepository<Account> accountRepository, IRepository<Payment> paymentRepository) {
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
        }

        /// <summary>
        ///     Balance of all relevant accounts at the end of the month.
        /// </summary>
        public double TotalBalance { get; set; }

        /// <summary>
        ///     Current Balance of all accounts.
        /// </summary>
        public double EndOfMonthBalance { get; set; }

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
        protected virtual double GetTotalBalance() => accountRepository.Data?.Sum(x => x.CurrentBalance) ?? 0;

        /// <summary>
        ///     Calculates the sum of all accounts at the end of the month.
        /// </summary>
        /// <returns>Sum of all balances including all payments to come till end of month.</returns>
        protected virtual double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedPayments = paymentRepository.Data
                .Where(p => !p.IsCleared)
                .Where(p => p.Date.Date <= Utilities.GetEndOfMonth());

            foreach (var payment in unclearedPayments)
            {
                //Transfer can be ignored since they don't change the summary.
                switch (payment.Type)
                {
                    case (int) PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case (int) PaymentType.Income:
                        balance += payment.Amount;
                        break;
                }
            }

            return balance;
        }
    }
}