using System;
using System.Collections.Generic;
using MoneyFox.Core.Model;

namespace MoneyFox.Core.Interfaces
{
    public interface IPaymentRepository : IRepository<PaymentViewModel>
    {
        /// <summary>
        ///     Delete the specified payment.
        /// </summary>
        /// <param name="paymentToDelete">Recurring payment to delete.</param>
        void DeleteRecurring(PaymentViewModel paymentToDelete);

        /// <summary>
        ///     Returns all uncleared payments.
        /// </summary>
        /// <returns>List of uncleared payments.</returns>
        IEnumerable<PaymentViewModel> GetUnclearedPayments();

        /// <summary>
        ///     Returns all uncleared payments up to the passed date.
        /// </summary>
        /// <param name="date">Date to which payments shall be selected.</param>
        /// <returns>List of uncleared payments.</returns>
        IEnumerable<PaymentViewModel> GetUnclearedPayments(DateTime date);

        /// <summary>
        ///     returns a list with payments who are related to this account.
        /// </summary>
        /// <param name="account">account to search the related</param>
        /// <returns>List of payments.</returns>
        IEnumerable<PaymentViewModel> GetRelatedPayments(Account account);

        /// <summary>
        ///     returns a list with payments who recure in a given timeframe
        /// </summary>
        /// <param name="filter">Expression to filter the selection.</param>
        /// <returns>List of recurring payments.</returns>
        IEnumerable<PaymentViewModel> LoadRecurringList(Func<PaymentViewModel, bool> filter = null);
    }
}