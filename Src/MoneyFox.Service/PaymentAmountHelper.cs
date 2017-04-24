using MoneyFox.Foundation;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Service
{
    public static class PaymentAmountHelper
    {
        public static void AddPaymentAmount(Payment payment)
        {
            //TODO: test this
            if (!payment.Data.IsCleared) return;

            if (payment.Data.Type == PaymentType.Transfer)
            {
                payment.Data.TargetAccount.CurrentBalance += payment.Data.Amount;
            }

            double AmountFunc(double x) => payment.Data.Type == PaymentType.Income
                ? x
                : -x;

            payment.Data.ChargedAccount.CurrentBalance += AmountFunc(payment.Data.Amount);
        }

        public static void RemovePaymentAmount(Payment payment)
        {
            //TODO: test this
            if (!payment.Data.IsCleared) return;

            if (payment.Data.Type == PaymentType.Transfer)
            {
                payment.Data.TargetAccount.CurrentBalance -= payment.Data.Amount;
            }

            double AmountFunc(double x) => payment.Data.Type == PaymentType.Income
                ? -x
                : x;

            payment.Data.ChargedAccount.CurrentBalance -= AmountFunc(payment.Data.Amount);
        }
    }
}
