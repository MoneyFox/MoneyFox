using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Infrastructure.Persistence.Configurations;

namespace MoneyFox.Infrastructure.Persistence
{
    /// <summary>
    ///     Represents the data context of the application
    /// </summary>
    public class EfCoreContext : DbContext, IEfCoreContext
    {
        private readonly IPublisher? publisher;
        private readonly ISettingsFacade? settingsFacade;
        
        public EfCoreContext(DbContextOptions options, IPublisher? publisher, ISettingsFacade? settingsFacade) : base(options)
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
    }
}