using MoneyFox.SharedKernel;

namespace MoneyFox.Core.Events
{
    internal sealed class PaymentCreatedEvent : BaseDomainEvent
    {
        public PaymentCreatedEvent(int paymentId)
        {
            PaymentId = paymentId;
        }

        public int PaymentId { get; }
    }
}