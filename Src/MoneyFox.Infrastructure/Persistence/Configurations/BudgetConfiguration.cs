namespace MoneyFox.Infrastructure.Persistence.Configurations
{
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
            builder.HasKey(c => c.Id);

            var splitStringConverter = new ValueConverter<IList<int>, string>(
                i => string.Join(";", i),
                s => string.IsNullOrWhiteSpace(s) ? new int[0] : s.Split(new[] { ';' }).Select(v => int.Parse(v)).ToArray());
            builder.Property(nameof(Budget.IncludedCategories)).HasConversion(splitStringConverter);
        }
    }

}
