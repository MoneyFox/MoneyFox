using System;
using System.Collections.Generic;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Manager
{
    public class EndOfMonthManager : IEndOfMonthManager
    {
        private readonly IPaymentRepository paymentRepository;

        public EndOfMonthManager(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        private int accountId = 0;

        public double GetEndOfMonthBalanceForAccount(Account account)
        {
            accountId = account.Id;
            var balance = account.CurrentBalance;
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

        private IEnumerable<Payment> LoadUnclearedPayments()
            => paymentRepository
                .GetList(p => !p.IsCleared)
                .Where(p => p.Date.Date <= Utilities.GetEndOfMonth())
                .Where(x => (x.ChargedAccountId == accountId)
                            || (x.TargetAccountId == accountId))
                .ToList();

        private double HandleTransferAmount(Payment payment, double balance)
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

        public void CheckEndOfMonthBalanceForAccounts(IEnumerable<Account> accounts)
        {
            foreach (var account in accounts)
            {
                account.EndMonthWarning = GetEndOfMonthBalanceForAccount(account) < 0
                    ? "Negative at end of month"
                    : " ";
            }
        }

        public double GetTotalEndOfMonthBalance(IEnumerable<Account> accounts)
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
