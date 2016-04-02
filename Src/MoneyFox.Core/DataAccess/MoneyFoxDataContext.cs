using Microsoft.Data.Entity;
using MoneyFox.Core.Constants;
using MoneyFox.Core.DatabaseModels;

namespace MoneyFox.Core.DataAccess
{
    /// <summary>
    ///     Provides an datacontext to access the moneyfox.sqlite database.
    /// </summary>
    public class MoneyFoxDataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RecurringPayment> RecurringPaymentViewModels { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={OneDriveConstants.DB_NAME}");
        }
    }
}