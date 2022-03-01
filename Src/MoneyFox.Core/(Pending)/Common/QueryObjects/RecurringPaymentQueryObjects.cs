namespace MoneyFox.Core._Pending_.Common.QueryObjects
{
    using Aggregates.Payments;
    using System;
    using System.Linq;

    public static class RecurringPaymentQueryObjects
    {
        public static IQueryable<RecurringPayment> IsNotExpired(this IQueryable<RecurringPayment> queryable) =>
            queryable.Where(x => x.IsEndless || x.EndDate >= DateTime.Today);
    }
}