using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.DataLayer.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> accountBuilder)
        {
            accountBuilder.HasIndex(b => b.Name);
        }
    }
}
