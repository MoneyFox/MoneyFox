using MoneyFox.Foundation;
using System;

namespace MoneyFox.Windows.Business
{
    public class LiveTilesPaymentInfo
    {
        public DateTime Mydate { get; set; }
        public double Myamount { get; set; }
        public PaymentType Type { get; set; }
        public string Chargeaccountname { get; set; }
    }
}