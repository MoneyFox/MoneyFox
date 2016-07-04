using System.Collections.Generic;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.ViewModels {
    /// <summary>
    ///     This ViewModel is for the usage in the paymentlist when a concret account is selected
    /// </summary>
    public class PaymentListBalanceViewModel : BalanceViewModel
    {
        private readonly int accountId;
        public PaymentListBalanceViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository, int accountId)
            : base(accountRepository, paymentRepository)
        {
            this.accountId = accountId;
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected override double GetTotalBalance() => AccountRepository.FindById(accountId).CurrentBalance;

        /// <summary>
        ///     Calculates the sum of the selected account at the end of the month.
        ///     This includes all payments coming until the end of month.
        /// </summary>
        /// <returns>Balance of the selected accont including all payments to come till end of month.</returns>
        protected override double GetEndOfMonthValue() {
            var balance = TotalBalance;
            var unclearedPayments = LoadUnclearedPayments();

            foreach (var payment in unclearedPayments) {
                switch (payment.Type) {
                    case (int) PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case (int) PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case (int) PaymentType.Transfer:
                        balance = HandleTransferAmount(payment, balance);
                        break;
                }
            }

            return balance;
        }

        private double HandleTransferAmount(Payment payment, double balance) {
            if (accountId == payment.ChargedAccountId) {
                balance -= payment.Amount;
            }
            else {
                balance += payment.Amount;
            }
            return balance;
        }

        private IEnumerable<Payment> LoadUnclearedPayments()
            => PaymentRepository.GetUnclearedPayments(Utilities.GetEndOfMonth())
                .Where(x => x.ChargedAccountId == accountId
                            || x.TargetAccountId == accountId)
                .ToList();
    }
}