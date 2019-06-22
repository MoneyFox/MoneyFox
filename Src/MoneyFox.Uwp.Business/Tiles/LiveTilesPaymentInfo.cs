using System;
using MoneyFox.Foundation;

namespace MoneyFox.Uwp.Business.Tiles
{
    public class LiveTilesPaymentInfo
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public PaymentType Type { get; set; }
        public string Chargeaccountname { get; set; }
    }
}