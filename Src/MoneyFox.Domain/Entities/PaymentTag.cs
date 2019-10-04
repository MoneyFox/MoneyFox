namespace MoneyFox.Domain.Entities
{
    public class PaymentTag
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
