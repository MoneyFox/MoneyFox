using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.Converter
{
    public class NativeAmountFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var noParens = (NumberFormatInfo) CultureInfo.CurrentUICulture.NumberFormat.Clone();
            noParens.CurrencyNegativePattern = 1;
            var currencyValue = (decimal) value;

            return currencyValue.ToString("C", noParens);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
