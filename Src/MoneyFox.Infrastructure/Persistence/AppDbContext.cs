namespace MoneyFox.Infrastructure.Persistence;

using System;
using System.Threading;
using System.Threading.Tasks;
using Converter;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Notifications.DatabaseChanged;
using Domain;
using Domain.Aggregates;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.BudgetAggregate;
using Domain.Aggregates.CategoryAggregate;
using Domain.Aggregates.LedgerAggregate;
using Domain.Aggregates.RecurringTransactionAggregate;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly IPublisher? publisher;
    private readonly ISettingsFacade? settingsFacade;

    public AppDbContext(DbContextOptions options, IPublisher? publisher, ISettingsFacade? settingsFacade) : base(options)
    {
        this.publisher = publisher;
        this.settingsFacade = settingsFacade;
    }

    public DbSet<Budget> Budgets { get; set; } = null!;

    public DbSet<Account> Accounts { get; set; } = null!;

    public DbSet<Payment> Payments { get; set; } = null!;

    public DbSet<RecurringPayment> RecurringPayments { get; set; } = null!;

    public DbSet<RecurringTransaction> RecurringTransactions { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Ledger> Ledgers { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.LastModified = DateTime.Now;

                    break;
                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.Now;

                    break;
            }
        }

        var changeCount = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        if (publisher == null || settingsFacade == null)
        {
            return changeCount;
        }

        // dispatch events only if save was successful
        if (changeCount > 0)
        {
            await publisher.Publish(notification: new DataBaseChanged.Notification(), cancellationToken: cancellationToken);
        }

        return changeCount;
    }

    public void MigrateDb()
    {
        Database.Migrate();
    }

    public void ReleaseLock()
    {
        SqliteConnection.ClearAllPools();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<Currency>().HaveConversion<CurrencyConverter>();
        configurationBuilder.Properties<DateOnly>().HaveConversion<DateOnlyConverter>().HaveColumnType("date");
        configurationBuilder.Properties<DateOnly?>().HaveConversion<NullableDateOnlyConverter>().HaveColumnType("date");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }

    [UsedImplicitly]
    private sealed class CurrencyConverter : ValueConverter<Currency, string>
    {
        public CurrencyConverter() : base(convertToProviderExpression: v => v.AlphaIsoCode, convertFromProviderExpression: v => Currencies.Get(v)) { }
    }
}
