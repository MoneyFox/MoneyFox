using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Converter
{
    internal class CurrencyConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new CultureInfo(value.ToString()).NumberFormat.CurrencySymbol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
