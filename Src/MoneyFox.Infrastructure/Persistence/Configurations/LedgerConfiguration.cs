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
        _ = builder.Property(b => b.ExcludeFromEndOfMonthSummary);

        _ = builder.OwnsOne<Money>(
            navigationExpression: l => l.CurrentBalance,
            buildAction: m =>
            {
                m.Property(p => p.Amount).HasColumnName("CurrentBalance");
                m.Property(p => p.Currency).HasColumnName("Currency");
            });

        _ = builder.Property(b => b.Created);
        _ = builder.Property(b => b.LastModified);

        _ = builder.OwnsMany(
            navigationExpression: ledger => ledger.Transactions,
            buildAction: t =>
            {
                _ = t.Property(i => i.Id)
                    .ValueGeneratedOnAdd()
                    .HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new(v));

                _ = t.Property(p => p.Type).IsRequired();
                _ = t.OwnsOne<Money>(
                    navigationExpression: l => l.Amount,
                    buildAction: m =>
                    {
                        m.Property(p => p.Amount).HasColumnName("Amount");
                        m.Property(p => p.Currency).HasColumnName("Currency");
                    });

                _ = t.Property(p => p.BookingDate).IsRequired();

                _ = builder.Property(b => b.Created);
                _ = builder.Property(b => b.LastModified);
            });
    }
}
