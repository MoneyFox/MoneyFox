using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation;
using MoneyFox.Service.DataServices;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Business.Manager
{
    /// <summary>
    ///     Provides different calculations for the balance at the end of month.
    /// </summary>
    public class EndOfMonthManager : IEndOfMonthManager
    {
        private readonly IPaymentService paymentService;

        public EndOfMonthManager(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        private int accountId = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<double> GetEndOfMonthBalanceForAccount(Account account)
        {
            accountId = account.Data.Id;
            var balance = account.Data.CurrentBalance;

            foreach (var payment in await paymentService.GetUnclearedPayments(Utilities.GetEndOfMonth(), accountId))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public async Task CheckIfAccountsAreOverdrawn(IEnumerable<Account> accounts)
        {
            foreach (var account in accounts)
            {
                account.Data.IsOverdrawn = await GetEndOfMonthBalanceForAccount(account) < 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public async Task<double> GetTotalEndOfMonthBalance(IEnumerable<Account> accounts)
        {
            var balance = accounts.Sum(x => x.Data.CurrentBalance);

            foreach (var payment in await paymentService.GetUnclearedPayments(Utilities.GetEndOfMonth()))
            {
                //Transfer can be ignored since they don't change the summary.
                switch (payment.Data.Type)
                {
                    case PaymentType.Expense:
                        balance -= payment.Data.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Data.Amount;
                        break;
                    case PaymentType.Transfer:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return balance;
        }
    }
}