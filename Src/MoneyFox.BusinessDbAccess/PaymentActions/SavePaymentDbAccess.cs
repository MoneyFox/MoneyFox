using System.Threading.Tasks;
using MoneyFox.DataLayer;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessDbAccess.PaymentActions
{
    public interface ISavePaymentDbAccess
    {
        Task AddPayment(Payment payment);
    }

    public class SavePaymentDbAccess : ISavePaymentDbAccess
    {
        private readonly EfCoreContext context;

        public SavePaymentDbAccess(EfCoreContext context)
        {
            this.context = context;
        }

        public async Task AddPayment(Payment payment)
        {
            await context.Payments.AddAsync(payment);
        }
    }
}
