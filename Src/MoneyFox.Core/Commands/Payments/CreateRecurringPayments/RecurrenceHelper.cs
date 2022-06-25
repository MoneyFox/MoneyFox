namespace MoneyFox.Core.Commands.Payments.CreateRecurringPayments
{

    using System;
    using ApplicationCore.Domain.Aggregates;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;
    using ApplicationCore.Domain.Exceptions;
    using Common.Extensions;
    using Resources;

    internal static class RecurrenceHelper
    {
        /// <summary>
        ///     Checks if the recurring PaymentViewModel is up for a repetition based on the passed PaymentViewModel
        /// </summary>
        /// <param name="payment">Last occurrence of the recurring payment.</param>
        /// <returns>True or False if the payment has to be repeated.</returns>
        public static bool CheckIfRepeatable(Payment payment)
        {
            if (!payment.IsCleared)
            {
                return false;
            }

            if (!payment.IsRecurring)
            {
                return false;
            }

            if (payment.RecurringPayment == null)
            {
                throw new RecurringPaymentNullException();
            }

            return CheckRecurrence(payment.RecurringPayment);
        }

        /// <summary>
        ///     Returns a boolean indicating whether the recurrence period of the supplied RecurringPayment object
        ///     has been exceeded and a new payment needs to be generated.
        /// </summary>
        /// <param name="recurringPayment">RecurringPayment object to be tested.</param>
        /// <returns>True if the duration since the LastRecurrenceCreated date of the RecurringPayment object exceeds the recurrence period, false otherwise.</returns>
        private static bool CheckRecurrence(RecurringPayment recurringPayment)
        {
            var currDate = DateTime.Today;
            var lastRecurCreated = recurringPayment.LastRecurrenceCreated.Date;
            switch (recurringPayment.Recurrence)
            {
                case PaymentRecurrence.Daily:
                    return currDate != lastRecurCreated;
                case PaymentRecurrence.DailyWithoutWeekend:
                    return currDate != lastRecurCreated && currDate.DayOfWeek != DayOfWeek.Saturday && currDate.DayOfWeek != DayOfWeek.Sunday;
                case PaymentRecurrence.Weekly:
                    return currDate.AddDays(-7) >= lastRecurCreated;
                case PaymentRecurrence.Biweekly:
                    return currDate.AddDays(-14) >= lastRecurCreated;
                case PaymentRecurrence.Monthly:
                    return currDate.AddMonths(-1).GetFirstDayOfMonth() >= lastRecurCreated.GetFirstDayOfMonth();
                case PaymentRecurrence.Bimonthly:
                    return currDate.AddMonths(-2).GetFirstDayOfMonth() >= lastRecurCreated.GetFirstDayOfMonth();
                case PaymentRecurrence.Quarterly:
                    return currDate.AddMonths(-3).GetFirstDayOfMonth() >= lastRecurCreated.GetFirstDayOfMonth();
                case PaymentRecurrence.Biannually:
                    return currDate.AddMonths(-6).GetFirstDayOfMonth() >= lastRecurCreated.GetFirstDayOfMonth();
                case PaymentRecurrence.Yearly:
                    return currDate.AddYears(-1).GetFirstDayOfMonth() >= lastRecurCreated.GetFirstDayOfMonth();
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Returns the Date of the next recurrence instance of the supplied RecurringPayment object.
        /// </summary>
        /// <param name="recurringPayment">RecurringPayment object to be evaluated.</param>
        /// <returns>Date of the next recurrence instance.</returns>
        public static DateTime GetPaymentDateFromRecurring(RecurringPayment recurringPayment)
        {
            var currDate = DateTime.Today;
            switch (recurringPayment.Recurrence)
            {
                case PaymentRecurrence.Daily:
                case PaymentRecurrence.Weekly:
                case PaymentRecurrence.Biweekly:
                    return currDate;
                case PaymentRecurrence.DailyWithoutWeekend:
                    if (currDate.DayOfWeek == DayOfWeek.Saturday || currDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        throw new InvalidOperationException(
                            $"Unable to create a {Strings.DailyWithoutWeekendLabel} recurring payment on a {currDate.DayOfWeek}");
                    }

                    return currDate;
                case PaymentRecurrence.Monthly:
                case PaymentRecurrence.Bimonthly:
                case PaymentRecurrence.Quarterly:
                case PaymentRecurrence.Biannually:
                case PaymentRecurrence.Yearly:
                    var dayOfMonth = recurringPayment.IsLastDayOfMonth
                        ? DateTime.DaysInMonth(year: currDate.Year, month: currDate.Month)
                        : Math.Min(val1: DateTime.DaysInMonth(year: currDate.Year, month: currDate.Month), val2: recurringPayment.StartDate.Day);

                    return new DateTime(year: currDate.Year, month: currDate.Month, day: dayOfMonth);
                default:
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(recurringPayment),
                        message: $"Unable to determine the payment date for recurrence type {recurringPayment.Recurrence}");
            }
        }
    }

}
