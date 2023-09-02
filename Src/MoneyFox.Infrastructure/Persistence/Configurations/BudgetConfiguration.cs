namespace MoneyFox.Infrastructure.Persistence.Configurations;

using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Aggregates.BudgetAggregate;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

[UsedImplicitly]
internal class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new(v));

        builder.OwnsOne(b => b.SpendingLimit).Property(sl => sl.Value).HasColumnName("SpendingLimit");
        builder.OwnsOne(b => b.Interval).Property(sl => sl.NumberOfMonths).HasColumnName("IntervalNumberOfMonths");
        builder.Property(nameof(Budget.IncludedCategories)).HasConversion(GetSplitStringConverter());
    }

    private static ValueConverter<IReadOnlyList<int>, string> GetSplitStringConverter()
    {
        return new(
            convertToProviderExpression: i => string.Join(";", i),
            convertFromProviderExpression: s
                => string.IsNullOrWhiteSpace(s) ? Array.Empty<int>() : s.Split(';', StringSplitOptions.None).Select(int.Parse).ToArray());
    }
}
