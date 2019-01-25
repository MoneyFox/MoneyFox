using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.DataLayer.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> categoryBuilder)
        {
            // Set FK from category to payment with cascade with cascade
            categoryBuilder
                .HasMany(m => m.Payments)
                .WithOne(t => t.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Set FK from category to recurring payment with cascade
            categoryBuilder
                .HasMany(m => m.RecurringPayments)
                .WithOne(t => t.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            categoryBuilder.HasIndex(b => b.Name);
        }
    }
}
