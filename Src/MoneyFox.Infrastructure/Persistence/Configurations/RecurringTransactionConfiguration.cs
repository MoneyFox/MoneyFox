namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Domain.Aggregates.RecurringTransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new(v));

        builder.Property(p => p.CategoryId);
        builder.Property(p => p.StartDate);
        builder.Property(p => p.EndDate);
        builder.Property(p => p.Amount);
        builder.Property(p => p.Type);
        builder.Property(p => p.Note);
        builder.Property(p => p.ChargedAccount);
        builder.Property(p => p.TargetAccount);
        builder.Property(p => p.Recurrence);
        builder.Property(p => p.IsLastDayOfMonth);
        builder.Property(p => p.LastRecurrence);
    }
}
