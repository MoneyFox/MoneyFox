using Microsoft.EntityFrameworkCore;
using MoneyFox.DataLayer.Configurations;
using MoneyFox.DataLayer.Entities;
using System;

namespace MoneyFox.DataLayer
{
    /// <summary>
    ///     Represents the data context of the application
    /// </summary>
    public class EfCoreContext : DbContext
    {
        public EfCoreContext()
        {
        }

        public EfCoreContext(DbContextOptions options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RecurringPayment> RecurringPayments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        ///     This is called when before the db is access.
        ///     Set DbPath before, so that we use here what db we have to use.
        /// </summary>
        /// <param name="optionsBuilder">Optionbuilder who is used.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Specify that we will use sqlite and the path of the database here
            optionsBuilder.UseSqlite($"Filename={DatabasePathHelper.GetDbPath()}");
        }

        /// <summary>
        ///     Called when the models are created.
        ///     Enables to configure advanced settings for the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ThrowIfNull(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentTagConfiguration());
        }

        private static void ThrowIfNull(ModelBuilder modelBuilder) { if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));  }
    }
}
