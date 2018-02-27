using System;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;

namespace MoneyFox.DataAccess
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
        public static RecurringPayment GetRecurringFromPayment(Payment payment,
                                                               bool isEndless,
                                                               PaymentRecurrence recurrence,
                                                               DateTime enddate = new DateTime())
            => new RecurringPayment
            {
                Data =
                {
                    ChargedAccount = payment.Data.ChargedAccount,
                    TargetAccount = payment.Data.TargetAccount,
                    StartDate = payment.Data.Date,
                    EndDate = !isEndless ? (DateTime?)enddate : null,
                    IsEndless = isEndless,
                    Amount = payment.Data.Amount,
                    Category = payment.Data.Category,
                    Type = payment.Data.Type,
                    Recurrence = recurrence,
                    Note = payment.Data.Note
                }
            };

        /// <summary>
        ///     Creates an PaymentViewModel based on the recurring PaymentViewModel.
        /// </summary>
        /// <param name="recurringPayment">The recurring PaymentViewModel the new PaymentViewModel shall be based on.</param>
        /// <returns>The new created PaymentViewModel</returns>
        public static Payment GetPaymentFromRecurring(RecurringPayment recurringPayment)
        {
            var date = DateTime.Today;

            //If the PaymentViewModel is monthly we want it on the same day of month again.
            if (recurringPayment.Data.Recurrence == PaymentRecurrence.Monthly)
            {
                date = DateTime.Today.AddDays(recurringPayment.Data.StartDate.Day - DateTime.Today.Day);

                double value = recurringPayment.Data.StartDate.Day;  //the Day value i.e. 31
                double max = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
                double difference = -(value - max);

                if (difference < 0)
                {
                    date = date.AddDays(difference);
                }
            }

            return new Payment(new PaymentEntity
            {
                ChargedAccount = recurringPayment.Data.ChargedAccount,
                ChargedAccountId = recurringPayment.Data.ChargedAccountId,
                TargetAccount = recurringPayment.Data.TargetAccount,
                TargetAccountId = recurringPayment.Data.TargetAccountId,
                Date = date,
                IsRecurring = true,
                Amount = recurringPayment.Data.Amount,
                Category = recurringPayment.Data.Category,
                CategoryId = recurringPayment.Data.CategoryId,
                Type = recurringPayment.Data.Type,
                RecurringPaymentId = recurringPayment.Data.Id,
                RecurringPayment = recurringPayment.Data,
                Note = recurringPayment.Data.Note
            });
        }

        /// <summary>
        ///     Checks if the recurring PaymentViewModel is up for a repetition based on the passed PaymentViewModel
        /// </summary>
        /// <param name="payment">Last occurence of the recurring payment.</param>
        /// <returns>True or False if the payment has to be repeated.</returns>
        public static bool CheckIfRepeatable(Payment payment)
        {
            if (!payment.Data.IsCleared)
            {
                return false;
            }

            switch (payment.Data.RecurringPayment.Recurrence)
            {
                case PaymentRecurrence.Daily:
                    return DateTime.Today.Date != payment.Data.Date.Date;

                case PaymentRecurrence.DailyWithoutWeekend:
                    return DateTime.Today.Date != payment.Data.Date.Date
                           && DateTime.Today.DayOfWeek != DayOfWeek.Saturday
                           && DateTime.Today.DayOfWeek != DayOfWeek.Sunday;

                case PaymentRecurrence.Weekly:
                    var daysWeekly = DateTime.Now - payment.Data.Date;
                    return daysWeekly.Days >= 7;

                case PaymentRecurrence.Biweekly:
                    var daysBiweekly = DateTime.Now - payment.Data.Date;
                    return daysBiweekly.Days >= 14;

                case PaymentRecurrence.Monthly:
                    return DateTime.Now.Month != payment.Data.Date.Month;

                case PaymentRecurrence.Bimonthly:
                    var date = DateTime.Now.AddMonths(-2);
                    return payment.Data.Date.Month <= date.Month && payment.Data.Date.Year == date.Year;

                case PaymentRecurrence.Quarterly:
                    return CheckQuarterly(payment);

                case PaymentRecurrence.Biannually:
                    return CheckBiannually(payment);

                case PaymentRecurrence.Yearly:
                    return DateTime.Now.Year != payment.Data.Date.Year
                           && DateTime.Now.Month >= payment.Data.Date.Month
                           || DateTime.Now.Year - payment.Data.Date.Year > 1;

                default:
                    return false;
            }
        }

        private static bool CheckQuarterly(Payment payment)
        {
            var dateDiff =  DateTime.Now - payment.Data.Date;
            return dateDiff.TotalDays >= 93;
        }

        private static bool CheckBiannually(Payment payment)
        {
            var dateDiff = DateTime.Now - payment.Data.Date;
            return dateDiff.TotalDays >= 184;
        }
    }
}
 