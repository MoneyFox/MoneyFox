namespace MoneyFox.Infrastructure.Persistence.Configurations
{

    using Core.Aggregates;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasIndex(b => b.Name);
            builder.Property(b => b.Name).IsRequired();
        }
    }

}
