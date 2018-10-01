using System;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.Business.Helpers
{
    public static class RecurringPaymentHelper
    {
        /// <summary>
        ///     Creates an recurring PaymentViewModel based on the Financial PaymentViewModel.
        /// </summary>
        /// <param name="payment">The financial PaymentViewModel the reuccuring shall be based on.</param>
        /// <param name="isEndless">If the recurrence is infinite or not.</param>
        /// <param name="recurrence">How often the PaymentViewModel shall be repeated.</param>
        /// <param name="enddate">Enddate for the recurring PaymentViewModel if it's not endless.</param>
        /// <returns>The new created recurring PaymentViewModel</returns>
        public static RecurringPaymentViewModel GetRecurringFromPayment(PaymentViewModel payment,
                bool isEndless,
                PaymentRecurrence recurrence,
                DateTime enddate = new DateTime())
            => new RecurringPaymentViewModel
            {
                Id = payment.RecurringPaymentId,
                ChargedAccount = payment.ChargedAccount,
                ChargedAccountId = payment.ChargedAccount.Id,
                TargetAccount = payment.TargetAccount,
                TargetAccountId = payment.TargetAccount?.Id ?? 0,
                StartDate = payment.Date,
                EndDate = enddate,
                IsEndless = isEndless,
                Amount = payment.Amount,
                CategoryId = payment.CategoryId,
                Category = payment.Category,
                Type = payment.Type,
                Recurrence = recurrence,
                Note = payment.Note
            };

        /// <summary>
        ///     Creates an PaymentViewModel based on the recurring PaymentViewModel.
        /// </summary>
        /// <param name="recurringPayment">The recurring PaymentViewModel the new PaymentViewModel shall be based on.</param>
        /// <returns>The new created PaymentViewModel</returns>
        public static PaymentViewModel GetPaymentFromRecurring(RecurringPaymentViewModel recurringPayment)
        {
            var date = DateTime.Today;

            //If the PaymentViewModel is monthly we want it on the same day of month again.
            if (recurringPayment.Recurrence == PaymentRecurrence.Monthly)
            {
                date = DateTime.Today.AddDays(recurringPayment.StartDate.Day - DateTime.Today.Day);

                double value = recurringPayment.StartDate.Day;  //the Day value i.e. 31
                double max = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
                double difference = -(value - max);

                if (difference < 0)
                {
                    date = date.AddDays(difference);
                }
            }

            return new PaymentViewModel
            {
                ChargedAccount = recurringPayment.ChargedAccount,
                ChargedAccountId = recurringPayment.ChargedAccountId,
                TargetAccount = recurringPayment.TargetAccount,
                TargetAccountId = recurringPayment.TargetAccountId,
                Date = date,
                IsRecurring = true,
                Amount = recurringPayment.Amount,
                Category = recurringPayment.Category,
                CategoryId = recurringPayment.CategoryId,
                Type = recurringPayment.Type,
                RecurringPaymentId = recurringPayment.Id,
                RecurringPayment = recurringPayment,
                Note = recurringPayment.Note
            };
        }

        /// <summary>
        ///     Checks if the recurring PaymentViewModel is up for a repetition based on the passed PaymentViewModel
        /// </summary>
        /// <param name="recurringPayment">Recurring PaymentViewModel to check.</param>
        /// <param name="relatedPayment">PaymentViewModel to compare.</param>
        /// <returns>True or False if the PaymentViewModel have to be repeated.</returns>
        public static bool CheckIfRepeatable(RecurringPaymentViewModel recurringPayment, PaymentViewModel relatedPayment)
        {
            if (!relatedPayment.IsCleared)
            {
                return false;
            }

            switch (recurringPayment.Recurrence)
            {
                case PaymentRecurrence.Daily:
                    return DateTime.Today.Date != relatedPayment.Date.Date;

                case PaymentRecurrence.DailyWithoutWeekend:
                    return (DateTime.Today.Date != relatedPayment.Date.Date)
                           && (DateTime.Today.DayOfWeek != DayOfWeek.Saturday)
                           && (DateTime.Today.DayOfWeek != DayOfWeek.Sunday);

                case PaymentRecurrence.Weekly:
                    var daysWeekly = DateTime.Now - relatedPayment.Date;
                    return daysWeekly.Days >= 7;

                case PaymentRecurrence.Biweekly:
                    var daysBiweekly = DateTime.Now - relatedPayment.Date;
                    return daysBiweekly.Days >= 14;

                case PaymentRecurrence.Monthly:
                    return DateTime.Now.Month != relatedPayment.Date.Month;

                case PaymentRecurrence.Bimonthly:
                    return relatedPayment.Date.Month <= DateTime.Now.AddMonths(-2).Month;

                case PaymentRecurrence.Yearly:
                    return ((DateTime.Now.Year != relatedPayment.Date.Year)
                            && (DateTime.Now.Month >= relatedPayment.Date.Month))
                           || (DateTime.Now.Year - relatedPayment.Date.Year > 1);

                default:
                    return false;
            }
        }
    }
}
 