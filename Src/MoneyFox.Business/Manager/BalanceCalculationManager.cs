using System;
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
    public interface IBalanceCalculationManager
    {
        /// <summary>
        ///     Checks all accounts if the end of month balance is below zero.
        ///     If yes the IsOverdrawn Property is set to true.
        /// </summary>
        /// <param name="accounts">Accounts to check.</param>
        Task CheckIfAccountsAreOverdrawn();

        /// <summary>
        ///     Returns the sum of all account balances that are not excluded.
        /// </summary>
        Task<double> GetTotalBalance();

        /// <summary>
        ///     Returns the sum of the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <returns>Sum of the end of month balance.</returns>
        Task<double> GetTotalEndOfMonthBalance();

        /// <summary>
        ///     Returns the the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <param name="account">Account to calculate the balance.</param>
        /// <returns>The end of month balance.</returns>
        Task<double> GetEndOfMonthBalanceForAccount(Account account);
    }

    public class BalanceCalculationManager : IBalanceCalculationManager
    {
        private readonly IPaymentService paymentService;
        private readonly IAccountService accountService;

        public BalanceCalculationManager(IPaymentService paymentService, IAccountService accountService)
        {
            this.paymentService = paymentService;
            this.accountService = accountService;
        }

        /// <inheritdoc />
        public async Task CheckIfAccountsAreOverdrawn()
        {
            foreach (var account in await accountService.GetAllAccounts())
            {
                account.Data.IsOverdrawn = await GetEndOfMonthBalanceForAccount(account) < 0;
            }
        }

        /// <inheritdoc />
        public async Task<double> GetTotalBalance()
        {
            var accounts = await accountService.GetNotExcludedAccounts();
            return accounts.Sum(x => x.Data.CurrentBalance);
        }

        /// <inheritdoc />
        public async Task<double> GetTotalEndOfMonthBalance()
        {
            var balance = await GetTotalBalance();
            var accounts = await accountService.GetAllAccounts();
            var notExcluded = await accountService.GetNotExcludedAccounts();
            var excluded = accounts.Except(notExcluded);

            foreach (var payment in await paymentService.GetUnclearedPayments(Utilities.GetEndOfMonth()))
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
                        if(excluded.Contains(payment.Data.ChargedAccountId)){ //Transfer from excluded account
                            balance += payment.Data.Amount; 
                        }
                        else
                            if(excluded.Contains(payment.Data.TargetAccountId)){ //Transfer to excluded account
                                balance -= payment.Data.Amount;
                            }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return balance;
        }

        /// <inheritdoc />
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