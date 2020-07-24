using MoneyFox.Domain.Entities;
using System;
using System.Linq;

namespace MoneyFox.Application.Common.QueryObjects
{
    public static class RecurringPaymentQueryObjects
    {
        public static IQueryable<RecurringPayment> IsNotExpired(this IQueryable<RecurringPayment> queryable)
        {
            return queryable.Where(x => x.IsEndless || x.EndDate >= DateTime.Today);
        }
    }
}
