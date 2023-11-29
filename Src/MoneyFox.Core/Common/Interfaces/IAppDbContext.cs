namespace MoneyFox.Core.Common.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.BudgetAggregate;
using Domain.Aggregates.CategoryAggregate;
using Domain.Aggregates.LedgerAggregate;
using Domain.Aggregates.RecurringTransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IAppDbContext : IDisposable
{
    DbSet<Account> Accounts { get; }

    DbSet<Ledger> Ledgers { get; }

    DbSet<Payment> Payments { get; }

    DbSet<RecurringPayment> RecurringPayments { get; }

    DbSet<RecurringTransaction> RecurringTransactions { get; }

    DbSet<Category> Categories { get; }

    DbSet<Budget> Budgets { get; }

    ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    void MigrateDb();
}
