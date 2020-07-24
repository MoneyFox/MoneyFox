using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoneyFox.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Common.Interfaces
{
    public interface IEfCoreContext
    {
        DbSet<Account> Accounts { get; }

        DbSet<Payment> Payments { get; }

        DbSet<RecurringPayment> RecurringPayments { get; }

        DbSet<Category> Categories { get; }

        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry Entry(object entity);

        void Dispose();
    }
}
