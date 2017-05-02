using System.IO;
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

        internal DbSet<AccountEntity> Accounts { get; set; }
        internal DbSet<PaymentEntity> Payments { get; set; }
        internal DbSet<RecurringPaymentEntity> RecurringPayments { get; set; }
        internal DbSet<CategoryEntity> Categories { get; set; }

        public static string DbPath { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set FK from payment to account for charged account with cascade
            modelBuilder.Entity<AccountEntity>()
                        .HasMany(m => m.ChargedPayments)
                        .WithOne(t => t.ChargedAccount)
                        .HasForeignKey(m => m.ChargedAccountId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Set FK from payment to account for target account with cascade
            modelBuilder.Entity<AccountEntity>()
                        .HasMany(m => m.TargetedPayments)
                        .WithOne(t => t.TargetAccount)
                        .HasForeignKey(m => m.TargetAccountId)
                        .OnDelete(DeleteBehavior.SetNull);

            // Set FK from recurring payment to account for charged account with cascade
            modelBuilder.Entity<AccountEntity>()
                        .HasMany(m => m.ChargedRecurringPayments)
                        .WithOne(t => t.ChargedAccount)
                        .HasForeignKey(m => m.ChargedAccountId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Set FK from recurring payment to account for target account with cascade
            modelBuilder.Entity<AccountEntity>()
                        .HasMany(m => m.TargetedRecurringPayments)
                        .WithOne(t => t.TargetAccount)
                        .HasForeignKey(m => m.TargetAccountId)
                        .OnDelete(DeleteBehavior.SetNull);

            // Set FK from category to payment with cascade with cascade
            modelBuilder.Entity<CategoryEntity>()
                        .HasMany(m => m.Payments)
                        .WithOne(t => t.Category)
                        .HasForeignKey(m => m.CategoryId)
                        .OnDelete(DeleteBehavior.SetNull);

            // Set FK from category to recurring payment with cascade
            modelBuilder.Entity<CategoryEntity>()
                        .HasMany(m => m.RecurringPayments)
                        .WithOne(t => t.Category)
                        .HasForeignKey(m => m.CategoryId)
                        .OnDelete(DeleteBehavior.SetNull);

            // Set FK from recurring payment to payment for charged account with cascade
            modelBuilder.Entity<RecurringPaymentEntity>()
                        .HasMany(m => m.RelatedPayments)
                        .WithOne(t => t.RecurringPayment)
                        .HasForeignKey(m => m.RecurringPaymentId)
                        .OnDelete(DeleteBehavior.SetNull);

            // Set Indizies
            modelBuilder.Entity<AccountEntity>()
                        .HasIndex(b => b.Name);
            modelBuilder.Entity<CategoryEntity>()
                        .HasIndex(b => b.Name);
        }
    }
}
