using MoneyFox.Core.Aggregates.Payments;
using System;
using System.Linq;

namespace MoneyFox.Core._Pending_.Common.QueryObjects
{
    public static class RecurringPaymentQueryObjects
    {
        public static IQueryable<RecurringPayment> IsNotExpired(this IQueryable<RecurringPayment> queryable) =>
            queryable.Where(x => x.IsEndless || x.EndDate >= DateTime.Today);
    }
}