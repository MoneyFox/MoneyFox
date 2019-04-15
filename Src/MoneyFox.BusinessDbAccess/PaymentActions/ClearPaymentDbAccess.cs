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
    ///     Provides operations to access the database for clear payment actions.
    /// </summary>
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
                .Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .AsQueryable()
                .AreNotCleared()
                .ToListAsync()
                ;
        }
    }
}
