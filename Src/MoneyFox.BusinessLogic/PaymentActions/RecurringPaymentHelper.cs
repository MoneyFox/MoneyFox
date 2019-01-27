using System;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Exceptions;

namespace MoneyFox.BusinessLogic.PaymentActions
{
    public static class RecurringPaymentHelper
    {
        /// <summary>
        ///     Checks if the recurring PaymentViewModel is up for a repetition based on the passed PaymentViewModel
        /// </summary>
        /// <param name="payment">Last occurence of the recurring payment.</param>
        /// <returns>True or False if the payment has to be repeated.</returns>
        public static bool CheckIfRepeatable(Payment payment)
        {
            if (!payment.IsCleared)
            {
                return false;
            }

            if (payment.IsRecurring && payment.RecurringPayment == null) throw new MoneyFoxRecurringPaymentNullException();

            switch (payment.RecurringPayment.Recurrence)
            {
                case PaymentRecurrence.Daily:
                    return DateTime.Today.Date != payment.Date.Date;

                case PaymentRecurrence.DailyWithoutWeekend:
                    return DateTime.Today.Date != payment.Date.Date
                           && DateTime.Today.DayOfWeek != DayOfWeek.Saturday
                           && DateTime.Today.DayOfWeek != DayOfWeek.Sunday;

                case PaymentRecurrence.Weekly:
                    var daysWeekly = DateTime.Now - payment.Date;
                    return daysWeekly.Days >= 7;

                case PaymentRecurrence.Biweekly:
                    var daysBiweekly = DateTime.Now - payment.Date;
                    return daysBiweekly.Days >= 14;

                case PaymentRecurrence.Monthly:
                    return DateTime.Now.Month != payment.Date.Month;

                case PaymentRecurrence.Bimonthly:
                    var date = DateTime.Now.AddMonths(-2);
                    return payment.Date.Month <= date.Month && payment.Date.Year == date.Year;

                case PaymentRecurrence.Quarterly:
                    return CheckQuarterly(payment);

                case PaymentRecurrence.Biannually:
                    return CheckBiannually(payment);

                case PaymentRecurrence.Yearly:
                    return DateTime.Now.Year != payment.Date.Year
                           && DateTime.Now.Month >= payment.Date.Month
                           || DateTime.Now.Year - payment.Date.Year > 1;

                default:
                    return false;
            }
        }

        private static bool CheckQuarterly(Payment payment)
        {
            var dateDiff = DateTime.Now - payment.Date;
            return dateDiff.TotalDays >= 93;
        }

        private static bool CheckBiannually(Payment payment)
        {
            var dateDiff = DateTime.Now - payment.Date;
            return dateDiff.TotalDays >= 184;
        }
    }
}
