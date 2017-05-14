using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.EntityOld;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Represents the datacontext of the application
    /// </summary>
    public class ApplicationContextOld : DbContext
    {
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Payment> Payments { get; set; }
        internal DbSet<RecurringPayment> RecurringPayments { get; set; }
        internal DbSet<Category> Categories { get; set; }

        public static string DbPath { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }
    }
}
