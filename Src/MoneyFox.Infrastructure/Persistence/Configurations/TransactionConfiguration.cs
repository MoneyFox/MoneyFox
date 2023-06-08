namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Domain.Aggregates.LedgerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(i => i.Id).ValueGeneratedOnAdd().HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new(v));
        builder.Property(p => p.LedgerId).IsRequired().HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new(v));
        builder.Property(p => p.Type).IsRequired();
        builder.OwnsOne(
            navigationExpression: l => l.Amount,
            buildAction: m =>
            {
                m.Property(p => p.Amount).HasColumnName("Amount");
                m.Property(p => p.Currency).HasColumnName("Currency");
            });

        builder.Property(p => p.BookingDate).IsRequired();
        builder.Property(b => b.Created);
        builder.Property(b => b.LastModified);
    }
}
