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
    public interface IEndOfMonthManager
    {
        /// <summary>
        ///     Checks all accounts if the end of month balance is below zero.
        ///     If yes the IsOverdrawn Property is set to true.
        /// </summary>
        /// <param name="accounts">Accounts to check.</param>
        Task CheckIfAccountsAreOverdrawn(IEnumerable<Account> accounts);

        /// <summary>
        ///     Returns the sum of the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <param name="account">Accounts to calculate the balance.</param>
        /// <returns>Sum of the end of month balance.</returns>
        Task<double> GetTotalEndOfMonthBalance(IEnumerable<Account> account);

        /// <summary>
        ///     Returns the the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <param name="account">Account to calculate the balance.</param>
        /// <returns>The end of month balance.</returns>
        Task<double> GetEndOfMonthBalanceForAccount(Account account);
    }

    public class EndOfMonthManager : IEndOfMonthManager
    {
        private readonly IPaymentService paymentService;

        public EndOfMonthManager(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        
        public async Task CheckIfAccountsAreOverdrawn(IEnumerable<Account> accounts)
        {
            foreach (var account in accounts)
            {
                account.Data.IsOverdrawn = await GetEndOfMonthBalanceForAccount(account) < 0;
            }
        }

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

        public async Task<double> GetEndOfMonthBalanceForAccount(Account account)
        {
            var balance = account.Data.CurrentBalance;

            foreach (var payment in await paymentService.GetUnclearedPayments(Utilities.GetEndOfMonth(), account.Data.Id))
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
                        balance = HandleTransferAmount(payment, balance, account.Data.Id);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return balance;
        }

        private double HandleTransferAmount(Payment payment, double balance, int accountId)
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
    }
}