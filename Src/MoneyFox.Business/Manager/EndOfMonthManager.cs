using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.Helpers;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Service.DataServices;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Business.Manager
{
    public class EndOfMonthManager : IEndOfMonthManager
    {
        private readonly IPaymentService paymentService;

        public EndOfMonthManager(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        private int accountId = 0;

        public async Task<double> GetEndOfMonthBalanceForAccount(AccountViewModel accountViewModel)
        {
            accountId = accountViewModel.Id;
            var balance = accountViewModel.CurrentBalance;

            foreach (var payment in await paymentService.GetUnclearedPayments(accountId, Utilities.GetEndOfMonth()))
            {
                switch (payment.Data.Type)
                {
                    case PaymentType.Expense:
                        balance -= payment.Data.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Data.Amount;
                        break;

                    case PaymentType.Transfer:
                        balance = HandleTransferAmount(payment, balance);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return balance;
        }

        private double HandleTransferAmount(Payment payment, double balance)
        {
            if (accountId == payment.Data.ChargedAccountId)
            {
                balance -= payment.Data.Amount;
            }
            else
            {
                balance += payment.Data.Amount;
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
                    case PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Amount;
                        break;
                }
            }
            return balance;
        }
    }
}