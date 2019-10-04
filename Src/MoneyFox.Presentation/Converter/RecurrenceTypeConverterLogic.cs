using MoneyFox.Application.Resources;
using MoneyFox.Domain;

namespace MoneyFox.Presentation.Converter
{
    public static class RecurrenceTypeConverterLogic
    {
        public static string GetStringForPaymentRecurrence(PaymentRecurrence passedEnum)
        {
            switch (passedEnum)
            {
                case PaymentRecurrence.Daily:
                    return Strings.DailyLabel;
                case PaymentRecurrence.DailyWithoutWeekend:
                    return Strings.DailyWithoutWeekendLabel;
                case PaymentRecurrence.Weekly:
                    return Strings.WeeklyLabel;
                case PaymentRecurrence.Biweekly:
                    return Strings.BiweeklyLabel;
                case PaymentRecurrence.Monthly:
                    return Strings.MonthlyLabel;
                case PaymentRecurrence.Bimonthly:
                    return Strings.BimonthlyLabel;
                case PaymentRecurrence.Quarterly:
                    return Strings.QuarterlyLabel;
                case PaymentRecurrence.Biannually:
                    return Strings.BiannuallyLabel;
                case PaymentRecurrence.Yearly:
                    return Strings.YearlyLabel;

                default:
                    return string.Empty;
            }
        }
    }
}
