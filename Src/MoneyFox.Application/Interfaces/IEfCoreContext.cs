using Microsoft.EntityFrameworkCore;
using MoneyFox.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;

namespace MoneyFox.Application.Interfaces
{
    public interface IEfCoreContext
    {
        DbSet<Account> Accounts { get; }
        DbSet<Payment> Payments { get; }
        DbSet<RecurringPayment> RecurringPayments { get; }
        DbSet<Category> Categories { get; }
        DbSet<PaymentTag> PaymentTags { get; }
        DbSet<Tag> Tags{ get; }

        Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
