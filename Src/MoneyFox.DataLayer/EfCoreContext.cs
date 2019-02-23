using Microsoft.EntityFrameworkCore;
using MoneyFox.DataLayer.Configurations;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.DataLayer
{
    /// <summary>
    ///     Represents the datacontext of the application
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

        /// <summary>
        ///     The Path to the db who shall be opened
        /// </summary>
        public static string DbPath { get; set; }

        /// <summary>
        ///     This is called when before the db is access.
        ///     Set DbPath before, so that we use here what db we have to use.
        /// </summary>
        /// <param name="optionsBuilder">Optionbuilder who is used.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

        /// <summary>
        ///     Called when the models are created.
        ///     Enables to configure advanced settings for the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
