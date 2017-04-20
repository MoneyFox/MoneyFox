using System.Collections.Generic;
using System.Threading.Tasks;
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
}