namespace MoneyFox.Core._Pending_.Common.QueryObjects
{

    using System;
    using System.Linq;
    using Aggregates.Payments;

    public static class RecurringPaymentQueryObjects
    {
        public static IQueryable<RecurringPayment> IsNotExpired(this IQueryable<RecurringPayment> queryable)
        {
            return queryable.Where(x => x.IsEndless || x.EndDate >= DateTime.Today);
        }
    }

}
