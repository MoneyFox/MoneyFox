using System.Threading.Tasks;
using MoneyFox.DataLayer;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessDbAccess.PaymentActions
{
    public interface ISavePaymentDbAccess
    {
        Task<Account> GetAccount(int id);
        Task<Category> GetCategory(int id);
        Task AddPayment(Payment payment);
        Task Save();
    }

    public class SavePaymentDbAccess : ISavePaymentDbAccess
    {
        private readonly EfCoreContext context;

        public SavePaymentDbAccess(EfCoreContext context)
        {
            this.context = context;
        }

        public async Task<Account> GetAccount(int id)
        {
            return await context.Accounts.FindAsync(id);
        }

        public async Task<Category> GetCategory(int id)
        {
            return await context.Categories.FindAsync(id);
        }

        public async Task AddPayment(Payment payment)
        {
            await context.Payments.AddAsync(payment);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
