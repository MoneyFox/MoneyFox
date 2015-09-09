using System;
using Windows.UI.Xaml.Data;
using MoneyManager.Foundation;
using MoneyManager.Localization;

namespace MoneyManager.Windows.Converter
{
    public class RecurrenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var enumInt = int.Parse(value.ToString());

            switch (enumInt)
            {
                case (int) TransactionRecurrence.Weekly:
                    return Strings.WeeklyLabel;

                case (int) TransactionRecurrence.Monthly:
                    return Strings.MonthlyLabel;

                case (int) TransactionRecurrence.Yearly:
                    return Strings.YearlyLabel;
            }

            return Strings.YearlyLabel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}