using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence.Configurations;

namespace MoneyFox.Persistence
{
    /// <summary>
    ///     Represents the data context of the application
    /// </summary>
    public class EfCoreContext : DbContext, IEfCoreContext
    {
        public EfCoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<RecurringPayment> RecurringPayments { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        ///     Called when the models are created.
        ///     Enables to configure advanced settings for the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentTagConfiguration());
        }
    }
}
