namespace MoneyFox.Infrastructure.Persistence
{

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core._Pending_.Common.Facades;
    using Core.ApplicationCore.Domain.Aggregates;
    using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
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

            var changeCount = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (publisher == null || settingsFacade == null)
            {
                return changeCount;
            }

            // dispatch events only if save was successful
            if (changeCount > 0)
            {
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }

            return changeCount;
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
