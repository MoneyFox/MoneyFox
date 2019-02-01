using System;
using System.Linq;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.ServiceLayer.QueryObject
{
    public static class PaymentQueries
    {
        /// <summary>
        ///     Adds a filter to a query for cleared payments
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for cleared payments.</returns>
        public static IQueryable<PaymentViewModel> AreCleared(this IQueryable<PaymentViewModel> query)
        {
            return query.Where(payment => payment.IsCleared);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who are not cleared
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for not cleared payments.</returns>
        public static IQueryable<PaymentViewModel> AreNotCleared(this IQueryable<PaymentViewModel> query)
        {
            return query.Where(payment => !payment.IsCleared);
        }
        
        /// <summary>
        ///     Adds a filter to a query for recurring payments
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for recurring payments.</returns>
        public static IQueryable<PaymentViewModel> AreRecurring(this IQueryable<PaymentViewModel> query)
        {
            return query.Where(payment => payment.IsRecurring);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a date smaller or equals to the passed date.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="date">Date to filter for.</param>
        /// <returns>Query filtered for the date.</returns>
        public static IQueryable<PaymentViewModel> HasDateSmallerEqualsThan(this IQueryable<PaymentViewModel> query, DateTime date)
        {
            return query.Where(payment => payment.Date.Date <= date.Date);
        }
        
        /// <summary>
        ///     Adds a filter to a query for payments who has a certain id as charged or target account.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="accountId">AccountId to filter for </param>
        /// <returns>Query filtered for the account id.</returns>
        public static IQueryable<PaymentViewModel> HasAccountId(this IQueryable<PaymentViewModel> query, int accountId)
        {
            return query.Where(payment => payment.ChargedAccount.Id == accountId || payment.TargetAccount != null && payment.TargetAccount.Id == accountId);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a certain id as charged account.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="accountId">AccountId to filter for </param>
        /// <returns>Query filtered for the account id.</returns>
        public static IQueryable<PaymentViewModel> HasChargedAccountId(this IQueryable<PaymentViewModel> query, int accountId)
        {
            return query.Where(payment => payment.ChargedAccount.Id == accountId);
        }

        /// <summary>
        ///     Orders a query descending by the date.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Ordered Query.</returns>
        public static IQueryable<PaymentViewModel> OrderDescendingByDate(this IQueryable<PaymentViewModel> query)
        {
            return query.OrderByDescending(x => x.Date);
        }


    }
}
