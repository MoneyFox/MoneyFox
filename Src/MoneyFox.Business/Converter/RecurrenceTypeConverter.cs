using System;
using System.Globalization;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MvvmCross.Converters;

namespace MoneyFox.Business.Converter
{
    /// <summary>
    ///     Converts the RecurrenceType to a string.
    /// </summary>
    public class RecurrenceTypeConverter : IMvxValueConverter
    {
        /// <summary>
        ///     Converts the passed recurrencetype to a string.
        /// </summary>
        /// <param name="value">Recurrencetype to convert.</param>
        /// <param name="targetType">Is not used.</param>
        /// <param name="parameter">Is not used.</param>
        /// <param name="culture">Is not used.</param>
        /// <returns>String for the RecurrenceType.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var passedEnum = (PaymentRecurrence) value;

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

        /// <summary>
        ///      Not implemented.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}