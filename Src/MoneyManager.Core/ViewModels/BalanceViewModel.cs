using System.Linq;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class BalanceViewModel : BaseViewModel, IBalanceViewModel
    {
        protected readonly IAccountRepository AccountRepository;
        protected readonly IPaymentRepository PaymentRepository;

        public BalanceViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository)
        {
            AccountRepository = accountRepository;
            PaymentRepository = paymentRepository;
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
        public MvxCommand UpdateBalanceCommand => new  MvxCommand(UpdateBalance);

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
        protected virtual double GetTotalBalance()
        {
            return AccountRepository.Data?.Sum(x => x.CurrentBalance) ?? 0;
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the end of the month.
        /// </summary>
        /// <returns>Sum of all balances including all payments to come till end of month.</returns>
        protected virtual double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedPayments = PaymentRepository.GetUnclearedPayments(Utilities.GetEndOfMonth());

            foreach (var payment in unclearedPayments)
            {
                //Transfer can be ignored since they don't change the summary.
                switch (payment.Type)
                {
                    case (int)PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case (int)PaymentType.Income:
                        balance += payment.Amount;
                        break;
                }
            }

            return balance;
        }
    }
}