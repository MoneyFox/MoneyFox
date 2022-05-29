namespace MoneyFox.Infrastructure.Persistence
{

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Aggregates;
    using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Core.Common.Facades;
    using Core.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

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

        public DbSet<Category> Categories { get; set; } = null!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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

            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (publisher == null || settingsFacade == null)
            {
                return result;
            }

            // dispatch events only if save was successful
            if (ChangeTracker.Entries().Any())
            {
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder?.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }

}
