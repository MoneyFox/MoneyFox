namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Domain.Aggregates.LedgerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
{
    public void Configure(EntityTypeBuilder<Ledger> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new(v));

        builder.HasIndex(b => b.Name);
        builder.Property(b => b.Name).IsRequired();
        builder.Property(b => b.Note);
        builder.Property(b => b.IsExcludeFromEndOfMonthSummary);

        builder.OwnsOne(
            navigationExpression: l => l.CurrentBalance,
            buildAction: m =>
            {
                m.Property(p => p.Amount).HasColumnName("CurrentBalance");
                m.Property(p => p.Currency).HasColumnName("Currency");
            });

        builder.Property(b => b.Created);
        builder.Property(b => b.LastModified);
    }
}
