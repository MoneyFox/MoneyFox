using System;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Helpers
{
    public static class RecurringPaymentHelper
    {
        /// <summary>
        ///     Creates an recurring Payment based on the Financial payment.
        /// </summary>
        /// <param name="payment">The financial payment the reuccuring shall be based on.</param>
        /// <param name="isEndless">If the recurrence is infinite or not.</param>
        /// <param name="recurrence">How often the payment shall be repeated.</param>
        /// <param name="enddate">Enddate for the recurring payment if it's not endless.</param>
        /// <returns>The new created recurring payment</returns>
        public static RecurringPayment GetRecurringFromPayment(Payment payment,
            bool isEndless,
            int recurrence,
            DateTime enddate = new DateTime())
        {
            return new RecurringPayment
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
        }

        /// <summary>
        ///     Creates an payment based on the recurring payment.
        /// </summary>
        /// <param name="recurringPayment">The recurring payment the new Payment shall be based on.</param>
        /// <returns>The new created payment</returns>
        public static Payment GetPaymentFromRecurring(RecurringPayment recurringPayment)
        {
            var date = DateTime.Today;

            //If the payment is monthly we want it on the same day of month again.
            if (recurringPayment.Recurrence == (int) PaymentRecurrence.Monthly)
            {
                date = DateTime.Today.AddDays(recurringPayment.StartDate.Day - DateTime.Today.Day);
            }

            return new Payment
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
        ///     Checks if the recurring payment is up for a repetition based on the passed Payment
        /// </summary>
        /// <param name="recurringPayment">Recurring payment to check.</param>
        /// <param name="relatedPayment">Payment to compare.</param>
        /// <returns>True or False if the payment have to be repeated.</returns>
        public static bool CheckIfRepeatable(RecurringPayment recurringPayment, Payment relatedPayment)
        {
            if (!relatedPayment.IsCleared) return false;

            switch (recurringPayment.Recurrence)
            {
                case (int) PaymentRecurrence.Daily:
                    return DateTime.Today.Date != relatedPayment.Date.Date;

                case (int) PaymentRecurrence.DailyWithoutWeekend:
                    return DateTime.Today.Date != relatedPayment.Date.Date
                           && DateTime.Today.DayOfWeek != DayOfWeek.Saturday
                           && DateTime.Today.DayOfWeek != DayOfWeek.Sunday;

                case (int) PaymentRecurrence.Weekly:
                    var daysWeekly = DateTime.Now - relatedPayment.Date;
                    return daysWeekly.Days >= 7;

                case (int) PaymentRecurrence.Biweekly:
                    var daysBiweekly = DateTime.Now - relatedPayment.Date;
                    return daysBiweekly.Days >= 14;

                case (int) PaymentRecurrence.Monthly:
                    return DateTime.Now.Month != relatedPayment.Date.Month;

                case (int) PaymentRecurrence.Yearly:
                    return (DateTime.Now.Year != relatedPayment.Date.Year
                           && DateTime.Now.Month >= relatedPayment.Date.Month)
                           || (DateTime.Now.Year - relatedPayment.Date.Year) > 1;

                default:
                    return false;
            }
        }
    }
}