using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;

namespace MoneyManager.Core.Converter
{
    public class DateTimeFormatConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime) value).ToString("D");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}