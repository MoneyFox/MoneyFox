using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using MoneyFox.Application.Common.QueryObjects;

namespace MoneyFox.BusinessDbAccess.PaymentActions
{
    /// <summary>
    ///     Provides operations to access the database for clear payment actions.
    /// </summary>
    public interface IClearPaymentDbAccess
    {
        Task<List<Payment>> GetUnclearedPaymentsAsync();
    }

    public class ClearPaymentDbAccess : IClearPaymentDbAccess
    {
        private readonly IEfCoreContextAdapter context;

        public ClearPaymentDbAccess(IEfCoreContextAdapter context)
        {
            this.context = context;
        }

        public async Task<List<Payment>> GetUnclearedPaymentsAsync()
        {
            return await context.Payments
                                .Include(x => x.ChargedAccount)
                                .Include(x => x.TargetAccount)
                                .AsQueryable()
                                .AreNotCleared()
                                .ToListAsync();
        }
    }
}
