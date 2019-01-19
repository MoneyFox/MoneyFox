using MoneyFox.Foundation;
using System;

namespace MoneyFox.Windows.Business
{
    public class LiveTilesPaymentInfo
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public PaymentType Type { get; set; }
        public string Chargeaccountname { get; set; }
    }
}