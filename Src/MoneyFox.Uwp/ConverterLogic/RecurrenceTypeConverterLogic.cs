using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Uwp.ConverterLogic
{
    public static class RecurrenceTypeConverterLogic
    {
        [SuppressMessage(
            "Critical Code Smell",
            "S1541:Methods and properties should not be too complex",
            Justification = "Switch")]
        public static string GetStringForPaymentRecurrence(PaymentRecurrence passedEnum) => passedEnum switch
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