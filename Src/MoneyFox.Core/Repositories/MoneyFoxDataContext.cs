using Microsoft.Data.Entity;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Model;

namespace MoneyFox.Core.Repositories
{
    public class MoneyFoxDataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RecurringPayment> RecurringPayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename=moneyfox.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Blog.Url required
            modelBuilder.Entity<Account>()
                .Property(b => b.Name)
                .IsRequired();
        }
    }
}
