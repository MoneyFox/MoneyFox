using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Converter
{
    public class CultureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
            {
                return string.Format(CultureInfo.CurrentCulture, formatString, value);
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}