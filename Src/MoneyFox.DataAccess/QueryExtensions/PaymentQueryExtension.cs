using System;
using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;

namespace MoneyFox.DataAccess.QueryExtensions
{
    /// <summary>
    ///     Provides Extensions for payments queries.
    /// </summary>
    public static class PaymentQueryExtensions
    {
        /// <summary>
        ///     Adds a filter to a query for cleared payments
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for cleared payments.</returns>
        public static IQueryable<PaymentEntity> AreCleared(this IQueryable<PaymentEntity> query)
        {
            return query.Where(payment => payment.IsCleared);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who are not cleared
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for not cleared payments.</returns>
        public static IQueryable<PaymentEntity> AreNotCleared(this IQueryable<PaymentEntity> query)
        {
            return query.Where(payment => !payment.IsCleared);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a date larger or equals to the passed date.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="date">Date to filter for.</param>
        /// <returns>Query filtered for not cleared payments.</returns>
        public static IQueryable<PaymentEntity> HasDateLargerEqualsThan(this IQueryable<PaymentEntity> query, DateTime date)
        {
            return query.Where(payment => payment.Date.Date >= date.Date);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a date smaller or equals to the passed date.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="date">Date to filter for.</param>
        /// <returns>Query filtered for the date.</returns>
        public static IQueryable<PaymentEntity> HasDateSmallerEqualsThan(this IQueryable<PaymentEntity> query, DateTime date)
        {
            return query.Where(payment => payment.Date.Date <= date.Date);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a certain id as charged or target account.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="accountId">AccountId to filter for </param>
        /// <returns>Query filtered for the account id.</returns>
        public static IQueryable<PaymentEntity> HasAccountId(this IQueryable<PaymentEntity> query, int accountId)
        {
            return query.Where(payment => payment.ChargedAccountId == accountId || payment.TargetAccountId == accountId);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a certain id as charged account id.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="accountId">AccountId to filter for </param>
        /// <returns>Query filtered for the charged account id.</returns>
        public static IQueryable<PaymentEntity> HasChargedAccountId(this IQueryable<PaymentEntity> query, int accountId)
        {
            return query.Where(payment => payment.ChargedAccountId == accountId);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a certain id as target account id.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="accountId">AccountId to filter for </param>
        /// <returns>Query filtered for the charged account id.</returns>
        public static IQueryable<PaymentEntity> HasTargetAccountId(this IQueryable<PaymentEntity> query, int accountId)
        {
            return query.Where(payment => payment.TargetAccountId == accountId);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who are not Transfers
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for payments who are not transfers.</returns>
        public static IQueryable<PaymentEntity> WithoutTransfers(this IQueryable<PaymentEntity> query)
        {
            return query.Where(payment => payment.Type != PaymentType.Transfer);
        }

        /// <summary>
        ///     Selects a <see cref="Payment"/> for every item in a query.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Selection query.</returns>
        public static IQueryable<Payment> SelectPayments(this IQueryable<PaymentEntity> query)
        {
            return query.Select(payment => new Payment(payment));
        }
    }
}
