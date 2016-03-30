using System;
using System.Collections.Generic;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;

namespace MoneyFox.Core.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        /// <summary>
        ///     Delete the specified payment.
        /// </summary>
        /// <param name="paymentToDelete">Recurring payment to delete.</param>
        void DeleteRecurring(Payment paymentToDelete);

        /// <summary>
        ///     Returns all uncleared payments.
        /// </summary>
        /// <returns>List of uncleared payments.</returns>
        IEnumerable<Payment> GetUnclearedPayments();

        /// <summary>
        ///     Returns all uncleared payments up to the passed date.
        /// </summary>
        /// <param name="date">Date to which payments shall be selected.</param>
        /// <returns>List of uncleared payments.</returns>
        IEnumerable<Payment> GetUnclearedPayments(DateTime date);

        /// <summary>
        ///     returns a list with payments who are related to this account.
        /// </summary>
        /// <param name="account">account to search the related</param>
        /// <returns>List of payments.</returns>
        IEnumerable<Payment> GetRelatedPayments(Account account);

        /// <summary>
        ///     returns a list with payments who recure in a given timeframe
        /// </summary>
        /// <param name="filter">Expression to filter the selection.</param>
        /// <returns>List of recurring payments.</returns>
        IEnumerable<Payment> LoadRecurringList(Func<Payment, bool> filter = null);
    }
}