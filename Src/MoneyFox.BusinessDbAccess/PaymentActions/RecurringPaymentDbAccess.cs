using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.BusinessDbAccess.QueryObjects;
using MoneyFox.Domain.Entities;

namespace MoneyFox.BusinessDbAccess.PaymentActions
{
    /// <summary>
    ///     Provides operations to access the database for recurring payment actions
    /// </summary>
    public interface IRecurringPaymentDbAccess
    {
        Task<List<RecurringPayment>> GetRecurringPaymentsAsync();

        Task SaveNewPaymentsAsync(List<Payment> payments);
    }

    public class RecurringPaymentDbAccess : IRecurringPaymentDbAccess
    {
        private readonly IEfCoreContext context;

        public RecurringPaymentDbAccess(IEfCoreContext context)
        {
            this.context = context;
        }

        public Task<List<RecurringPayment>> GetRecurringPaymentsAsync()
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

        public async Task SaveNewPaymentsAsync(List<Payment> payments)
        {
            await context.Payments
                         .AddRangeAsync(payments);
        }
    }
}
