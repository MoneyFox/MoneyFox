using System.Collections.Generic;
using System.Linq;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;

namespace MoneyFox.Business.Manager
{
    public class EndOfMonthManager : IEndOfMonthManager
    {
        private readonly IPaymentRepository paymentRepository;

        public EndOfMonthManager(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        private int accountId = 0;

        public double GetEndOfMonthBalanceForAccount(AccountViewModel accountViewModel)
        {
            accountId = accountViewModel.Id;
            var balance = accountViewModel.CurrentBalance;
            var unclearedPayments = LoadUnclearedPayments();

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

        private IEnumerable<PaymentViewModel> LoadUnclearedPayments()
            => paymentRepository
                .GetList(p => !p.IsCleared)
                .Where(p => p.Date.Date <= Utilities.GetEndOfMonth())
                .Where(x => (x.ChargedAccountId == accountId)
                            || (x.TargetAccountId == accountId))
                .ToList();

        private double HandleTransferAmount(PaymentViewModel payment, double balance)
        {
            if (accountId == payment.ChargedAccountId)
            {
                balance -= payment.Amount;
            }
            else
            {
                balance += payment.Amount;
            }
            return balance;
        }

        public void CheckEndOfMonthBalanceForAccounts(IEnumerable<AccountViewModel> accounts)
        {
            foreach (var account in accounts)
            {
                account.IsOverdrawn = GetEndOfMonthBalanceForAccount(account) < 0;
            }
        }

        public double GetTotalEndOfMonthBalance(IEnumerable<AccountViewModel> accounts)
        {
            var balance = accounts.Sum(x => x.CurrentBalance);
            var unclearedPayments = paymentRepository
                .GetList(p => !p.IsCleared && (p.Date.Date <= Utilities.GetEndOfMonth()));

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