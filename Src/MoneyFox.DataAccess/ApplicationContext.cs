using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Represents the datacontext of the application
    /// </summary>
    public class ApplicationContext : DbContext, IDbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions options) : base(options) { }

        internal DbSet<AccountEntity> Accounts { get; set; }
        internal DbSet<PaymentEntity> Payments { get; set; }
        internal DbSet<RecurringPaymentEntity> RecurringPayments { get; set; }
        internal DbSet<CategoryEntity> Categories { get; set; }

        /// <summary>
        ///     The Path to the db who shall be opened
        /// </summary>
        public static string DbPath { get; set; }

        /// <summary>
        ///     This is called when before the db is access.
        ///     Set DbPath before, so that we use here what db we have to use.
        /// </summary>
        /// <param name="optionsBuilder">Optionbuilder who is used.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

        /// <summary>
        ///     Called when the models are created.
        ///     Enables to configure advanced settings for the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
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
                        .OnDelete(DeleteBehavior.Cascade);

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
                        .OnDelete(DeleteBehavior.Cascade);

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
