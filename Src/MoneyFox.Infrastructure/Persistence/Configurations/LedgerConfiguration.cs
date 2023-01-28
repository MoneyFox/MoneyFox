namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Domain;
using Domain.Aggregates.LedgerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
{
    public void Configure(EntityTypeBuilder<Ledger> builder)
    {
        _ = builder.HasKey(b => b.Id);
        _ = builder.Property(i => i.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new(v));

        _ = builder.HasIndex(b => b.Name);
        _ = builder.Property(b => b.Name).IsRequired();

        _ = builder.Property(b => b.Note);
        _ = builder.Property(b => b.IsExcluded);

        _ = builder.OwnsOne<Money>(
            i => i.CurrentBalance,
            m =>
            {
                m.Property(p => p.Amount).HasColumnName("CurrentBalance");
                m.Property(p => p.Currency).HasColumnName("Currency");
            });
    }
}
