namespace MoneyFox.Core.Common.Interfaces
{
    using Aggregates;
    using Aggregates.Payments;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAppDbContext : IDisposable
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
    }
}