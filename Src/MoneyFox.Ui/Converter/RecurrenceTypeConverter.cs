namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Common.ConverterLogic;
using Domain.Aggregates.AccountAggregate;
using Resources.Strings;

/// <summary>
///     Converts the RecurrenceType to a string.
/// </summary>
public class RecurrenceTypeConverter : IValueConverter
{
    /// <summary>
    ///     Converts the passed recurrence type to a string.
    /// </summary>
    /// <param name="value">Recurrence type to convert.</param>
    /// <param name="targetType">Is not used.</param>
    /// <param name="parameter">Is not used.</param>
    /// <param name="culture">Is not used.</param>
    /// <returns>String for the RecurrenceType.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var passedEnum = (PaymentRecurrence)value;

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

    /// <summary>
    ///     Not implemented.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
