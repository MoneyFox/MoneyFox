using System;
using MoneyFox.Core.Model;

namespace MoneyFox.Core.Helpers
{
    public static class RecurringPaymentHelper
    {
        /// <summary>
        ///     Creates an recurring PaymentViewModel based on the Financial PaymentViewModel.
        /// </summary>
        /// <param name="paymentViewModel">The financial PaymentViewModel the reuccuring shall be based on.</param>
        /// <param name="isEndless">If the recurrence is infinite or not.</param>
        /// <param name="recurrence">How often the PaymentViewModel shall be repeated.</param>
        /// <param name="enddate">Enddate for the recurring PaymentViewModel if it's not endless.</param>
        /// <returns>The new created recurring PaymentViewModel</returns>
        public static RecurringPaymentViewModel GetRecurringFromPaymentViewModel(PaymentViewModel paymentViewModel,
            bool isEndless,
            PaymentRecurrence recurrence,
            DateTime enddate = new DateTime())
        {
            return new RecurringPaymentViewModel
            {
                ChargedAccount = paymentViewModel.ChargedAccount,
                TargetAccount = paymentViewModel.TargetAccount,
                StartDate = paymentViewModel.Date,
                EndDate = enddate,
                IsEndless = isEndless,
                Amount = paymentViewModel.Amount,
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
        public static PaymentViewModel GetPaymentFromRecurring(RecurringPayment recurringPayment)
        {
            var date = DateTime.Today;
            var recurringVm = new RecurringPaymentViewModel(recurringPayment);

            //If the PaymentViewModel is monthly we want it on the same day of month again.
            if (recurringVm.Recurrence == PaymentRecurrence.Monthly)
            {
                date = DateTime.Today.AddDays(recurringVm.StartDate.Day - DateTime.Today.Day);
            }

            return new PaymentViewModel
            {
                ChargedAccount = recurringVm.ChargedAccount,
                TargetAccount = recurringVm.TargetAccount,
                Date = date,
                IsRecurring = true,
                Amount = recurringVm.Amount,
                Category = recurringVm.Category,
                Type = recurringVm.Type,
                RecurringPayment = recurringVm.GetRecurringPayment(),
                Note = recurringVm.Note
            };
        }

        //TODO: check if can be used with a recurring payment view model.
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

            switch (new RecurringPaymentViewModel(recurringPayment).Recurrence)
            {
                case PaymentRecurrence.Daily:
                    return DateTime.Today.Date != relatedPaymentViewModel.Date.Date;

                case PaymentRecurrence.DailyWithoutWeekend:
                    return DateTime.Today.Date != relatedPaymentViewModel.Date.Date
                           && DateTime.Today.DayOfWeek != DayOfWeek.Saturday
                           && DateTime.Today.DayOfWeek != DayOfWeek.Sunday;

                case PaymentRecurrence.Weekly:
                    var daysWeekly = DateTime.Now - relatedPaymentViewModel.Date;
                    return daysWeekly.Days >= 7;

                case PaymentRecurrence.Biweekly:
                    var daysBiweekly = DateTime.Now - relatedPaymentViewModel.Date;
                    return daysBiweekly.Days >= 14;

                case PaymentRecurrence.Monthly:
                    return DateTime.Now.Month != relatedPaymentViewModel.Date.Month;

                case PaymentRecurrence.Yearly:
                    return (DateTime.Now.Year != relatedPaymentViewModel.Date.Year
                            && DateTime.Now.Month >= relatedPaymentViewModel.Date.Month)
                           || DateTime.Now.Year - relatedPaymentViewModel.Date.Year > 1;

                default:
                    return false;
            }
        }
    }
}