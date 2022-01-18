using MoneyFox.Application.Resources;
using MoneyFox.Core;

namespace MoneyFox.ConverterLogic
{
    public static class RecurrenceTypeConverterLogic
    {
        public static string GetStringForPaymentRecurrence(PaymentRecurrence passedEnum) =>
            passedEnum switch
            {
                PaymentRecurrence.Daily => Strings.DailyLabel,
                PaymentRecurrence.DailyWithoutWeekend => Strings.DailyWithoutWeekendLabel,
                PaymentRecurrence.Weekly => Strings.WeeklyLabel,
                PaymentRecurrence.Biweekly => Strings.BiweeklyLabel,
                PaymentRecurrence.Monthly => Strings.MonthlyLabel,
                PaymentRecurrence.Bimonthly => Strings.BimonthlyLabel,
                PaymentRecurrence.Quarterly => Strings.QuarterlyLabel,
                PaymentRecurrence.Biannually => Strings.BiannuallyLabel,
                PaymentRecurrence.Yearly => Strings.YearlyLabel,
                _ => string.Empty
            };
    }
}