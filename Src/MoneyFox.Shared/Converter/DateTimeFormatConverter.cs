using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace MoneyFox.Shared.Converter
{
    public class DateTimeFormatConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((DateTime)value).ToString("D");

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => System.Convert.ToDateTime(value).ToString("d");
    }
}