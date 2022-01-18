using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFox.Core.Aggregates.Accounts;

namespace MoneyFox.Infrastructure.Persistence.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder) => builder.HasIndex(b => b.Name);
    }
}