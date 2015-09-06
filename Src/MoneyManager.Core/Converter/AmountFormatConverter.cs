using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;

namespace MoneyManager.Core.Converter
{
    public class AmountFormatConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}