namespace MoneyFox.Core.Common.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Domain.Aggregates;
using ApplicationCore.Domain.Aggregates.AccountAggregate;
using ApplicationCore.Domain.Aggregates.BudgetAggregate;
using ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IAppDbContext : IDisposable
{
    DbSet<Account> Accounts { get; }

    DbSet<Payment> Payments { get; }

    DbSet<RecurringPayment> RecurringPayments { get; }

    DbSet<Category> Categories { get; }

    DbSet<Budget> Budgets { get; }

    ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    EntityEntry Entry(object entity);

    void Migratedb();

    void ReleaseLock();
}
