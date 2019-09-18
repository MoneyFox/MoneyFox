using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.DataLayer.Configurations
{
    internal class PaymentTagConfiguration : IEntityTypeConfiguration<PaymentTag>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<PaymentTag> builder)
        {
            builder.HasKey(pt => new {pt.PaymentId, pt.TagId});

            builder.HasOne(pt => pt.Payment)
                   .WithMany(p => p.PaymentTags)
                   .HasForeignKey(pt => pt.PaymentId);

            builder.HasOne(pt => pt.Tag)
                   .WithMany(t => t.PaymentTags)
                   .HasForeignKey(pt => pt.TagId);
        }
    }
}
