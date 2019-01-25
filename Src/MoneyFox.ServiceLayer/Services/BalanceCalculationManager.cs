using System;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.Business.Manager
{
    /// <summary>
    ///     Provides different calculations for the balance at the end of month.
    /// </summary>
    public interface IBalanceCalculationService
    {
        /// <summary>
        ///     Checks all accounts if the end of month balance is below zero.
        ///     If yes the IsOverdrawn Property is set to true.
        /// </summary>
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
        double GetEndOfMonthBalanceForAccount(AccountViewModel account);
    }

    /// <inheritdoc />
    public class BalanceCalculationService : IBalanceCalculationService
    {
        private readonly ICrudServicesAsync crudServices;

        /// <summary>
        ///     Constructor
        /// </summary>
        public BalanceCalculationService(ICrudServicesAsync crudServices)
        {
            this.crudServices = crudServices;
        }

        /// <inheritdoc />
        public async Task CheckIfAccountsAreOverdrawn()
        {
            foreach (var account in await crudServices.ReadManyNoTracked<AccountViewModel>().ToListAsync())
                account.IsOverdrawn = GetEndOfMonthBalanceForAccount(account) < 0;
        }

        /// <inheritdoc />
        public async Task<double> GetTotalBalance()
        {
            return await crudServices.ReadManyNoTracked<AccountViewModel>()
                .AreNotExcluded()
                .SumAsync(x => x.CurrentBalance);
        }

        /// <inheritdoc />
        public async Task<double> GetTotalEndOfMonthBalance()
        {
            var excluded = await crudServices.ReadManyNoTracked<AccountViewModel>()
                .AreNotExcluded()
                .ToListAsync();
            var balance = await GetTotalBalance();

            foreach (var payment in crudServices
                .ReadManyNoTracked<PaymentViewModel>()
                .HasDateSmallerEqualsThan(Utilities.GetEndOfMonth()))

                switch (payment.Type)
                {
                    case PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case PaymentType.Transfer:
                        foreach (var i in excluded)
                        {
                            if (Equals(i, payment.ChargedAccount.Id))
                            {
                                //Transfer from excluded account
                                balance += payment.Amount;
                                break;
                            }

                            if (Equals(i, payment.TargetAccount.Id))
                            {
                                //Transfer to excluded account
                                balance -= payment.Amount;
                                break;
                            }
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            return balance;
        }

        /// <inheritdoc />
        public double GetEndOfMonthBalanceForAccount(AccountViewModel account)
        {
            var balance = account.CurrentBalance;

            foreach (var payment in crudServices.ReadManyNoTracked<PaymentViewModel>()
                .HasAccountId(account.Id)
                .HasDateSmallerEqualsThan(Utilities.GetEndOfMonth()))
                switch (payment.Type)
                {
                    case PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case PaymentType.Transfer:
                        balance = HandleTransferAmount(payment, balance, account.Id);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            return balance;
        }

        private double HandleTransferAmount(PaymentViewModel payment, double balance, int accountId)
        {
            if (accountId == payment.ChargedAccountId)
                balance -= payment.Amount;
            else
                balance += payment.Amount;
            return balance;
        }
    }
}