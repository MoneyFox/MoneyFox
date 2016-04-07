using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace MoneyFox.Shared.Converter
{
    public class DateTimeFormatConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime) value).ToString("D");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDateTime(value).ToString("d");
        }
    }
}