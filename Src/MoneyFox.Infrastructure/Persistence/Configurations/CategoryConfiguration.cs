namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        _ = builder.HasKey(c => c.Id);
        _ = builder.Property(c => c.Name).IsRequired();
        _ = builder.HasIndex(c => c.Name);
        _ = builder.HasMany(c => c.Payments).WithOne(t => t.Category!).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.SetNull);
    }
}
