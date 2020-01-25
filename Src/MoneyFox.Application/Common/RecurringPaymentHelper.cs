using System;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;

namespace MoneyFox.Application.Common
{
    public static class RecurringPaymentHelper
    {
        private const int WEEKLY_RECURRENCE_DAYS = 7;
        private const int BIWEEKLY_RECURRENCE_DAYS = 14;
        private const int QUARTERLY_RECURRENCE_DAYS = 93;
        private const int BIANNUALLY_RECURRENCE_DAYS = 184;
        private const int BIMONTHLY_RECURRENCE_MONTHS = -2;

        /// <summary>
        ///     Checks if the recurring PaymentViewModel is up for a repetition based on the passed PaymentViewModel
        /// </summary>
        /// <param name="payment">Last occurrence of the recurring payment.</param>
        /// <returns>True or False if the payment has to be repeated.</returns>
        public static bool CheckIfRepeatable(Payment payment)
        {
            if (!payment.IsCleared) return false;
            if (!payment.IsRecurring) return false;
            if (payment.RecurringPayment == null) throw new RecurringPaymentNullException();

            switch (payment.RecurringPayment.Recurrence)
            {
                case PaymentRecurrence.Daily:
                    return DateTime.Today.Date != payment.Date.Date;

                case PaymentRecurrence.DailyWithoutWeekend:
                    return DateTime.Today.Date != payment.Date.Date
                           && DateTime.Today.DayOfWeek != DayOfWeek.Saturday
                           && DateTime.Today.DayOfWeek != DayOfWeek.Sunday;

                case PaymentRecurrence.Weekly:
                    TimeSpan daysWeekly = DateTime.Now - payment.Date;

                    return daysWeekly.Days >= WEEKLY_RECURRENCE_DAYS;

                case PaymentRecurrence.Biweekly:
                    TimeSpan daysBiweekly = DateTime.Now - payment.Date;

                    return daysBiweekly.Days >= BIWEEKLY_RECURRENCE_DAYS;

                case PaymentRecurrence.Monthly:
                    return DateTime.Now.Month != payment.Date.Month;

                case PaymentRecurrence.Bimonthly:
                    DateTime date = DateTime.Now.AddMonths(BIMONTHLY_RECURRENCE_MONTHS);

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
            TimeSpan dateDiff = DateTime.Now - payment.Date;
            return dateDiff.TotalDays >= QUARTERLY_RECURRENCE_DAYS;
        }

        private static bool CheckBiannually(Payment payment)
        {
            TimeSpan dateDiff = DateTime.Now - payment.Date;

            return dateDiff.TotalDays >= BIANNUALLY_RECURRENCE_DAYS;
        }

        public static DateTime GetPaymentDateFromRecurring(RecurringPayment recurringPayment)
        {
            if (recurringPayment.Recurrence == PaymentRecurrence.Monthly)
            {
                DateTime date = DateTime.Today.AddDays(recurringPayment.StartDate.Day - DateTime.Today.Day);

                int value = recurringPayment.StartDate.Day; //the Day value i.e. 31
                int max = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
                double difference = -(value - max);

                if (difference < 0) date = date.AddDays(difference);

                return date;
            }

            return DateTime.Today;
        }
    }
}
