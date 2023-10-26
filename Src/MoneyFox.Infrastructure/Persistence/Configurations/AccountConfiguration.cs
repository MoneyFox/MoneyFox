namespace MoneyFox.Infrastructure.Persistence.Configurations;

using Domain.Aggregates.AccountAggregate;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[UsedImplicitly]
internal class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Name);
        builder.Property(b => b.Name).IsRequired();
    }
}
