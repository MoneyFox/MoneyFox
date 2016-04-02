using Microsoft.Data.Entity;
using MoneyFox.Core.Constants;
using MoneyFox.Core.DatabaseModels.Old;

namespace MoneyFox.Core.DataAccess
{
    /// <summary>
    ///     Provides an datacontext to access the moneyfox.sqlite database.
    ///     THIS IS ONLY USED TO UPGRADE THE OLD TO THE NEW DATABASE.
    /// </summary>
    public class MoneyFoxOldDataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RecurringPayment> RecurringPayments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={OneDriveConstants.DB_NAME_OLD}");
        }
    }
}