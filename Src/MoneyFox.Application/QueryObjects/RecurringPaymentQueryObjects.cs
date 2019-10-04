using System;
using System.Linq;
using MoneyFox.Domain.Entities;

namespace MoneyFox.BusinessDbAccess.QueryObjects
{
    public static class RecurringPaymentQueryObjects
    {
        public static IQueryable<RecurringPayment> IsNotExpired(this IQueryable<RecurringPayment> queryable)
        {
            return queryable.Where(x => x.IsEndless || x.EndDate >= DateTime.Today);
        }
    }
}
