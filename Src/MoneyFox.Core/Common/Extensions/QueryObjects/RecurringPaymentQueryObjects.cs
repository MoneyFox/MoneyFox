namespace MoneyFox.Core.Common.Extensions.QueryObjects;

using System;
using System.Linq;
using ApplicationCore.Domain.Aggregates;

public static class RecurringPaymentQueryObjects
{
    public static IQueryable<RecurringPayment> IsNotExpired(this IQueryable<RecurringPayment> queryable)
    {
        return queryable.Where(x => x.IsEndless || x.EndDate >= DateTime.Today);
    }
}
