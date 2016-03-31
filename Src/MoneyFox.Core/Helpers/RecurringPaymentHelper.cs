using System;
using MoneyFox.Core.Model;

namespace MoneyFox.Core.Helpers
{
    public static class RecurringPaymentViewModelHelper
    {
        /// <summary>
        ///     Creates an recurring PaymentViewModel based on the Financial PaymentViewModel.
        /// </summary>
        /// <param name="paymentViewModel">The financial PaymentViewModel the reuccuring shall be based on.</param>
        /// <param name="isEndless">If the recurrence is infinite or not.</param>
        /// <param name="recurrence">How often the PaymentViewModel shall be repeated.</param>
        /// <param name="enddate">Enddate for the recurring PaymentViewModel if it's not endless.</param>
        /// <returns>The new created recurring PaymentViewModel</returns>
        public static RecurringPayment GetRecurringFromPaymentViewModel(PaymentViewModel paymentViewModel,
            bool isEndless,
            int recurrence,
            DateTime enddate = new DateTime())
        {
            return new RecurringPayment
            {
                Id = paymentViewModel.RecurringPayment.Id,
                ChargedAccount = paymentViewModel.ChargedAccount,
                ChargedAccountId = paymentViewModel.ChargedAccount.Id,
                TargetAccount = paymentViewModel.TargetAccount,
                TargetAccountId = paymentViewModel.TargetAccount?.Id ?? 0,
                StartDate = paymentViewModel.Date,
                EndDate = enddate,
                IsEndless = isEndless,
                Amount = paymentViewModel.Amount,
                CategoryId = paymentViewModel.Category.Id,
                Category = paymentViewModel.Category,
                Type = paymentViewModel.Type,
                Recurrence = recurrence,
                Note = paymentViewModel.Note
            };
        }

        /// <summary>
        ///     Creates an PaymentViewModel based on the recurring PaymentViewModel.
        /// </summary>
        /// <param name="recurringPayment">The recurring PaymentViewModel the new PaymentViewModel shall be based on.</param>
        /// <returns>The new created PaymentViewModel</returns>
        public static PaymentViewModel GetPaymentViewModelFromRecurring(RecurringPayment recurringPayment)
        {
            var date = DateTime.Today;

            //If the PaymentViewModel is monthly we want it on the same day of month again.
            if (recurringPayment.Recurrence == (int) PaymentViewModelRecurrence.Monthly)
            {
                date = DateTime.Today.AddDays(recurringPayment.StartDate.Day - DateTime.Today.Day);
            }

            return new PaymentViewModel
            {
                ChargedAccount = recurringPayment.ChargedAccount,
                TargetAccount = recurringPayment.TargetAccount,
                Date = date,
                IsRecurring = true,
                Amount = recurringPayment.Amount,
                Category = recurringPayment.Category,
                Type = recurringPayment.Type,
                RecurringPayment = recurringPayment,
                Note = recurringPayment.Note
            };
        }

        /// <summary>
        ///     Checks if the recurring PaymentViewModel is up for a repetition based on the passed PaymentViewModel
        /// </summary>
        /// <param name="recurringPayment">Recurring PaymentViewModel to check.</param>
        /// <param name="relatedPaymentViewModel">PaymentViewModel to compare.</param>
        /// <returns>True or False if the PaymentViewModel have to be repeated.</returns>
        public static bool CheckIfRepeatable(RecurringPayment recurringPayment, PaymentViewModel relatedPaymentViewModel)
        {
            if (!relatedPaymentViewModel.IsCleared)
            {
                return false;
            }

            switch (recurringPayment.Recurrence)
            {
                case (int) PaymentViewModelRecurrence.Daily:
                    return DateTime.Today.Date != relatedPaymentViewModel.Date.Date;

                case (int) PaymentViewModelRecurrence.DailyWithoutWeekend:
                    return DateTime.Today.Date != relatedPaymentViewModel.Date.Date
                           && DateTime.Today.DayOfWeek != DayOfWeek.Saturday
                           && DateTime.Today.DayOfWeek != DayOfWeek.Sunday;

                case (int) PaymentViewModelRecurrence.Weekly:
                    var daysWeekly = DateTime.Now - relatedPaymentViewModel.Date;
                    return daysWeekly.Days >= 7;

                case (int) PaymentViewModelRecurrence.Biweekly:
                    var daysBiweekly = DateTime.Now - relatedPaymentViewModel.Date;
                    return daysBiweekly.Days >= 14;

                case (int) PaymentViewModelRecurrence.Monthly:
                    return DateTime.Now.Month != relatedPaymentViewModel.Date.Month;

                case (int) PaymentViewModelRecurrence.Yearly:
                    return (DateTime.Now.Year != relatedPaymentViewModel.Date.Year
                            && DateTime.Now.Month >= relatedPaymentViewModel.Date.Month)
                           || DateTime.Now.Year - relatedPaymentViewModel.Date.Year > 1;

                default:
                    return false;
            }
        }
    }
}