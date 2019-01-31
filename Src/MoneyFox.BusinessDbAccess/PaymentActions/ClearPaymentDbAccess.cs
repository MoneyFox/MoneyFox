using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.BusinessDbAccess.QueryObjects;
using MoneyFox.DataLayer;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.BusinessDbAccess.PaymentActions
{
    public interface IClearPaymentDbAccess
    {
        Task<List<Payment>> GetUnclearedPayments();
    }

    public class ClearPaymentDbAccess : IClearPaymentDbAccess
    {
        private readonly EfCoreContext context;

        public ClearPaymentDbAccess(EfCoreContext context)
        {
            this.context = context;
        }

        public async Task<List<Payment>> GetUnclearedPayments()
        {
            return await context.Payments
                .AsQueryable()
                .AreNotCleared()
                .ToListAsync();
        }
    }
}
