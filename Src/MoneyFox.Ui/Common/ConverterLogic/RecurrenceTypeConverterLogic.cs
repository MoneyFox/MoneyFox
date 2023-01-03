namespace MoneyFox.Ui.Common.ConverterLogic;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Resources.Strings;

public static class RecurrenceTypeConverterLogic
{
    public static string GetStringForPaymentRecurrence(PaymentRecurrence passedEnum)
    {
        return passedEnum switch
        {
            PaymentRecurrence.Daily => Translations.DailyLabel,
            PaymentRecurrence.DailyWithoutWeekend => Translations.DailyWithoutWeekendLabel,
            PaymentRecurrence.Weekly => Translations.WeeklyLabel,
            PaymentRecurrence.Biweekly => Translations.BiweeklyLabel,
            PaymentRecurrence.Monthly => Translations.MonthlyLabel,
            PaymentRecurrence.Bimonthly => Translations.BimonthlyLabel,
            PaymentRecurrence.Quarterly => Translations.QuarterlyLabel,
            PaymentRecurrence.Biannually => Translations.BiannuallyLabel,
            PaymentRecurrence.Yearly => Translations.YearlyLabel,
            _ => string.Empty
        };
    }
}
