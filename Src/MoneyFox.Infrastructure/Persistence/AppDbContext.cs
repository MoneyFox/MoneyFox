namespace MoneyFox.Infrastructure.Persistence
{
    using Core._Pending_.Common.Facades;
    using Core.Aggregates;
    using Core.Aggregates.Payments;
    using Core.Common.Interfaces;
    using Core.Events;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly IPublisher? publisher;
        private readonly ISettingsFacade? settingsFacade;

        public AppDbContext(
            DbContextOptions options,
            IPublisher? publisher,
            ISettingsFacade? settingsFacade) : base(options)
        {
            this.publisher = publisher;
            this.settingsFacade = settingsFacade;
        }

        public DbSet<Account> Accounts { get; set; } = null!;

        public DbSet<Payment> Payments { get; set; } = null!;

        public DbSet<RecurringPayment> RecurringPayments { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder?.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach(EntityEntry<EntityBase>? entry in ChangeTracker.Entries<EntityBase>())
            {
                switch(entry.State)
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

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if(publisher == null || settingsFacade == null)
            {
                return result;
            }

            // dispatch events only if save was successful
            if(ChangeTracker.Entries().Any())
            {
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                await publisher.Publish(new DbEntityModifiedEvent(), cancellationToken);
            }

            return result;
        }

        public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();
    }
}