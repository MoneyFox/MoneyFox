using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.BusinessDbAccess.PaymentActions
{
    public interface ISavePaymentDbAccess
    {
        Task<Payment> GetPaymentByIdAsync(int id);

        void DeletePayment(Payment payment);

        Task DeleteRecurringPaymentAsync(int id);

        Task<List<Payment>> GetPaymentsForRecurringAsync(int recurringPaymentId);
    }

    public class SavePaymentDbAccess : ISavePaymentDbAccess
    {
        private readonly IEfCoreContext context;

        public SavePaymentDbAccess(IEfCoreContext context)
        {
            this.context = context;
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await context.Payments
                                .Include(x => x.ChargedAccount)
                                .Include(x => x.ChargedAccount)
                                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void DeletePayment(Payment payment)
        {
            context.Payments.Remove(payment);
        }

        public async Task DeleteRecurringPaymentAsync(int id)
        {
            context.RecurringPayments.Remove(await context.RecurringPayments.FindAsync(id));
        }

        public async Task<List<Payment>> GetPaymentsForRecurringAsync(int recurringPaymentId)
        {
            return await context.Payments
                                .Where(x => x.IsRecurring)
                                .Where(x => x.RecurringPayment.Id == recurringPaymentId)
                                .ToListAsync();
        }
    }
}
