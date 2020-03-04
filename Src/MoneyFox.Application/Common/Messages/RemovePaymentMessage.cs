namespace MoneyFox.Application.Common.Messages
{
    /// <summary>
    /// Message to remove the passed message.
    /// </summary>
    public class RemovePaymentMessage
    {
        public RemovePaymentMessage(int paymentId)
        {
            PaymentId = paymentId;
        }

        public int PaymentId { get; }
    }
}
