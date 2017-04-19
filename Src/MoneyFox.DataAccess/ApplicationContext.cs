using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.Foundation.Constants;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Represents the datacontext of the application
    /// </summary>
    public class ApplicationContext : DbContext
    {
        //public static string DataBasePath { get; set; }

        internal DbSet<AccountEntity> Users { get; set; }
        internal DbSet<PaymentEntity> Payments { get; set; }
        internal DbSet<RecurringPaymentEntity> RecurringPayments { get; set; }
        internal DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabaseConstants.DB_NAME}");
        }
    }
}
