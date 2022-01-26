using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Events;
using MoneyFox.Infrastructure.Persistence.Configurations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Infrastructure.Persistence
{
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

        /// <summary>
        ///     Called when the models are created. Enables to configure advanced settings for the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
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