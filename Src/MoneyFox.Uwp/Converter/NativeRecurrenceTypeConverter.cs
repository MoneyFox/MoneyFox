using System;
using Windows.UI.Xaml.Data;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Uwp.Converter
{
    public class NativeRecurrenceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var passedEnum = (PaymentRecurrence)value;

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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}