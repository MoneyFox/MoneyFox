using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace MoneyFox.Shared.ViewModels
{
    /// <summary>
    ///     This ViewModel is for the usage in the paymentlist when a concret account is selected
    /// </summary>
    public class PaymentListBalanceViewModel : BalanceViewModel
    {
        public PaymentListBalanceViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository)
            : base(accountRepository, paymentRepository)
        {
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected override double GetTotalBalance() => AccountRepository.Selected.CurrentBalance;

        /// <summary>
        ///     Calculates the sum of the selected account at the end of the month.
        ///     This includes all payments coming until the end of month.
        /// </summary>
        /// <returns>Balance of the selected accont including all payments to come till end of month.</returns>
        protected override double GetEndOfMonthValue()
        {
            var balance = TotalBalance;
            var unclearedPayments = LoadUnclreadPayments();

            foreach (var payment in unclearedPayments)
            {
                switch (payment.Type)
                {
                    case (int)PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case (int)PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case (int)PaymentType.Transfer:
                        balance = HandleTransferAmount(payment, balance);
                        break;
                }
            }

            return balance;
        }

        private double HandleTransferAmount(Payment payment, double balance)
        {
            if (AccountRepository.Selected == payment.ChargedAccount)
            {
                balance -= payment.Amount;
            }
            else
            {
                balance += payment.Amount;
            }
            return balance;
        }

        private IEnumerable<Payment> LoadUnclreadPayments()
            => PaymentRepository.GetUnclearedPayments(Utilities.GetEndOfMonth())
                .Where(x => x.ChargedAccountId == AccountRepository.Selected.Id
                            || x.TargetAccountId == AccountRepository.Selected.Id)
                .ToList();
    }
}