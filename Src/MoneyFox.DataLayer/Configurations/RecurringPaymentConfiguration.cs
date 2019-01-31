using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.DataLayer.Configurations
{
    internal class RecurringPaymentConfiguration : IEntityTypeConfiguration<RecurringPayment>
    {
        public void Configure(EntityTypeBuilder<RecurringPayment> recurringPaymentBuilder)
        {
            // Set FK from recurring payment to payment for charged account with cascade
            //recurringPaymentBuilder
            //    .HasMany(m => m.RelatedPayments)
            //    .WithOne(t => t.RecurringPayment)
            //    .HasForeignKey(m => m.RecurringPaymentId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
