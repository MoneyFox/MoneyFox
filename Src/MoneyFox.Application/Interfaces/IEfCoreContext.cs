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

        ChangeTracker ChangeTracker { get; }

        Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        EntityEntry Entry(object entity);

    }
}
