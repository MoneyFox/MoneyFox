namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Domain.Aggregates.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        _ = builder.HasKey(b => b.Id);
        _ = builder.HasIndex(b => b.Name);
        _ = builder.Property(b => b.Name).IsRequired();
    }
}
