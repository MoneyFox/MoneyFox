using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.DataLayer.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> accountBuilder)
        {
            // Set FK from payment to account for charged account with cascade
            accountBuilder
                .HasMany(m => m.ChargedPayments)
                .WithOne(t => t.ChargedAccount)
                .HasForeignKey(m => m.ChargedAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set FK from payment to account for target account with cascade
            accountBuilder
                .HasMany(m => m.TargetedPayments)
                .WithOne(t => t.TargetAccount)
                .HasForeignKey(m => m.TargetAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set FK from recurring payment to account for charged account with cascade
            accountBuilder
                .HasMany(m => m.ChargedRecurringPayments)
                .WithOne(t => t.ChargedAccount)
                .HasForeignKey(m => m.ChargedAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set FK from recurring payment to account for target account with cascade
            accountBuilder
                .HasMany(m => m.TargetedRecurringPayments)
                .WithOne(t => t.TargetAccount)
                .HasForeignKey(m => m.TargetAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            accountBuilder.HasIndex(b => b.Name);
        }
    }
}
