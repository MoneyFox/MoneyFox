using System;
using System.Linq;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessDbAccess.QueryObjects
{
    /// <summary>
    ///     Provides extensions for recurring payment queries.
    /// </summary>
    public static class RecurringPaymentQueryObjects
    {
        /// <summary>
        ///     Filters if recurring payment is not expired yet.
        /// </summary>
        /// <param name="queryable">Queryable</param>
        /// <returns>Filtered queryable.</returns>
        public static IQueryable<RecurringPayment> IsNotExpired(this IQueryable<RecurringPayment> queryable)
        {
            return queryable.Where(x => x.IsEndless || x.EndDate >= DateTime.Today);
        }
    }
}
