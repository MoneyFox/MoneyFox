using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.BusinessDbAccess.QueryObjects;
using MoneyFox.DataLayer;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessDbAccess.PaymentActions
{
    /// <summary>
    ///     Provides operations to access the database for recurring payment actions
    /// </summary>
    public interface IRecurringPaymentDbAccess
    {
        Task<List<RecurringPayment>> GetRecurringPayments();

        Task SaveNewPayments(List<Payment> payments);
    }

    public class RecurringPaymentDbAccess : IRecurringPaymentDbAccess
    {
        private readonly EfCoreContext context;

        public RecurringPaymentDbAccess(EfCoreContext context)
        {
            this.context = context;
        }

        public Task<List<RecurringPayment>> GetRecurringPayments()
        {
            return context.RecurringPayments
                .Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .Include(x => x.RelatedPayments)
                .AsQueryable()
                .IsNotExpired()
                .ToListAsync();
        }

        public async Task SaveNewPayments(List<Payment> payments)
        {
            await context.Payments
                         .AddRangeAsync(payments)
                         .ConfigureAwait(false);
        }
    }
}
