namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        _ = builder.HasKey(b => b.Id);
    }
}
