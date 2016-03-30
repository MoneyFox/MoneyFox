using Microsoft.Data.Entity;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;

namespace MoneyFox.Core.DataAccess
{
    /// <summary>
    ///     Provides an datacontext to access the moneyfox.sqlite database.
    /// </summary>
    public class MoneyFoxDataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RecurringPayment> RecurringPayments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=monefox.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Blog.Url required
            modelBuilder.Entity<Account>()
                .Property(a => a.Name)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .Property(p => p.ChargedAccountId)
                .IsRequired();

            modelBuilder.Entity<RecurringPayment>()
                .Property(p => p.ChargedAccountId)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired();
        }
    }
}