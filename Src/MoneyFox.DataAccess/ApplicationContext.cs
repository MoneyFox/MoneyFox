using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>()
                        .HasMany(m => m.ChargedPayments)
                        .WithOne(t => t.ChargedAccount)
                        .HasForeignKey(m => m.ChargedAccountId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AccountEntity>()
                        .HasMany(m => m.TargetedPayments)
                        .WithOne(t => t.TargetAccount)
                        .HasForeignKey(m => m.TargetAccountId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
