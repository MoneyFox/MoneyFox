namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Core.ApplicationCore.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class RecurringPaymentConfiguration : IEntityTypeConfiguration<RecurringPayment>
{
    public void Configure(EntityTypeBuilder<RecurringPayment> builder)
    {
        _ = builder.HasKey(b => b.Id);
    }
}
