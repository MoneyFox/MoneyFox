namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Domain;
using Domain.Aggregates.RecurringTransactionAggregate;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[UsedImplicitly]
internal sealed class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.HasIndex(rt => rt.RecurringTransactionId).IsUnique();
        builder.Property(rt => rt.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new(v));

        builder.Property(rt => rt.RecurringTransactionId).IsRequired();
        builder.Property(rt => rt.CategoryId);
        builder.Property(rt => rt.StartDate);
        builder.Property(rt => rt.EndDate);
        builder.OwnsOne<Money>(
            navigationExpression: rt => rt.Amount,
            buildAction: o =>
            {
                o.Property(p => p.Amount).HasColumnName("Amount");
                o.Property(p => p.Currency).HasColumnName("Currency");
            });

        builder.Property(rt => rt.Note);
        builder.Property(rt => rt.ChargedAccountId);
        builder.Property(rt => rt.TargetAccountId);
        builder.Property(rt => rt.Recurrence);
        builder.Property(rt => rt.IsLastDayOfMonth);
        builder.Property(rt => rt.LastRecurrence);
    }
}
