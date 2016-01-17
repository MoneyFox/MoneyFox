using System;
using System.Collections.Generic;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces
{
    public interface ITransactionRepository : IRepository<Payment>
    {
        /// <summary>
        ///     Returns all Uncleared Transactions.
        /// </summary>
        /// <returns>List of uncleared transactions.</returns>
        IEnumerable<Payment> GetUnclearedTransactions();

        /// <summary>
        ///     Returns all uncleared transaction up to the passed date.
        /// </summary>
        /// <param name="date">Date to which transactions shall be selected.</param>
        /// <returns>List of uncleared transactions.</returns>
        IEnumerable<Payment> GetUnclearedTransactions(DateTime date);

        /// <summary>
        ///     returns a list with transactions who are related to this account
        /// </summary>
        /// <param name="account">account to search the related</param>
        /// <returns>List of transactions</returns>
        IEnumerable<Payment> GetRelatedTransactions(Account account);

        /// <summary>
        ///     returns a list with transaction who recure in a given timeframe
        /// </summary>
        /// <param name="filter">Expression to filter the selection.</param>
        /// <returns>List of recurring transactions</returns>
        IEnumerable<Payment> LoadRecurringList(Func<Payment, bool> filter = null);
    }
}