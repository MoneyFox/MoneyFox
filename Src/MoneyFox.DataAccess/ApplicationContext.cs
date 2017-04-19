using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Represents the datacontext of the application
    /// </summary>
    public class ApplicationContext : DbContext
    {
        public static string DataBasePath { get; set; }

        internal DbSet<Account> Users { get; set; }
        internal DbSet<Payment> Payments { get; set; }
        internal DbSet<RecurringPayment> RecurringPayments { get; set; }
        internal DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DataBasePath}");
        }
    }
}
