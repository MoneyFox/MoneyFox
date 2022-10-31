namespace MoneyFox.Infrastructure.Persistence.Configurations;

using System;
using System.Collections.Generic;
using System.Linq;
using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

internal class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        _ = builder.HasKey(c => c.Id);
        _ = builder.OwnsOne(b => b.SpendingLimit).Property(sl => sl.Value).HasColumnName("SpendingLimit");
        _ = builder.Property(nameof(Budget.IncludedCategories)).HasConversion(GetSplitStringConverter());
    }

    private static ValueConverter<IReadOnlyList<int>, string> GetSplitStringConverter()
    {
        return new(
            convertToProviderExpression: i => string.Join(";", i),
            convertFromProviderExpression: s => string.IsNullOrWhiteSpace(s) ? Array.Empty<int>() : s.Split(new[] { ';' }).Select(int.Parse).ToArray());
    }
}
