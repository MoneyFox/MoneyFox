using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using MoneyFox.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using MoneyFox.DataLayer;
using MoneyFox.DataLayer.Configurations;

namespace MoneyFox.Persistence
{
    public interface IEfCoreContext
    {
        DbSet<Account> Accounts { get; }
        DbSet<Payment> Payments { get; }
        DbSet<RecurringPayment> RecurringPayments { get; }
        DbSet<Category> Categories { get; }

        Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    ///     Represents the data context of the application
    /// </summary>
    public class EfCoreContext : DbContext, IEfCoreContext
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
        }

        private void ThrowIfNull(ModelBuilder modelBuilder) { if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder)); }
    }
}
