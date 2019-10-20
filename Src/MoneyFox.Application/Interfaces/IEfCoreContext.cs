using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Interfaces
{
    public interface IEfCoreContext
    {
        DbSet<Account> Accounts { get; }
        DbSet<Payment> Payments { get; }
        DbSet<RecurringPayment> RecurringPayments { get; }
        DbSet<Category> Categories { get; }
        DbSet<PaymentTag> PaymentTags { get; }
        DbSet<Tag> Tags { get; }

        Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
