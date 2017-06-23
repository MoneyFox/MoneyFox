using System;
using MoneyFox.Foundation;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Service
{
    /// <summary>
    ///     Provides helper functions for payment amount.
    /// </summary>
    public static class PaymentAmountHelper
    {
        /// <summary>
        ///     If the payment is an expense it subtracts the amount from the charged account.
        ///     if it is an income it adds it to the target account.
        ///     IMPORTANT: Payment has to be cleared. Otherwise no action will take place.
        /// </summary>
        /// <param name="payment">Payment.</param>
        public static void AddPaymentAmount(Payment payment)
        {
            if (!payment.Data.IsCleared) return;

            if (payment.Data.Type == PaymentType.Transfer)
            {
                payment.Data.TargetAccount.CurrentBalance += payment.Data.Amount;
            }

            Func<double, double> amountFunc = x =>
                payment.Data.Type == PaymentType.Income
                    ? x
                    : -x;

            payment.Data.ChargedAccount.CurrentBalance += amountFunc(payment.Data.Amount);
        }

        /// <summary>
        ///     If the payment is an expense it adds the amount from the charged account.
        ///     if it is an income it subtracts it to the target account.
        ///     IMPORTANT: Payment has to be cleared. Otherwise no action will take place.
        /// </summary>
        /// <param name="payment">Payment.</param>
        public static void RemovePaymentAmount(Payment payment)
        {
            if (!payment.Data.IsCleared) return;

            if (payment.Data.Type == PaymentType.Transfer)
            {
                payment.Data.TargetAccount.CurrentBalance -= payment.Data.Amount;
            }

            Func<double, double> amountFunc = x =>
                payment.Data.Type == PaymentType.Income
                    ? -x
                    : x;

            payment.Data.ChargedAccount.CurrentBalance += amountFunc(payment.Data.Amount);
        }
    }
}
