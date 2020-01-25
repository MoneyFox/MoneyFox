using System;
using Windows.UI.Xaml.Data;
using MoneyFox.Application;

namespace MoneyFox.Uwp.Converter
{
    public class AmountFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var currencyValue = (decimal) value;
            return currencyValue.ToString("C", CultureHelper.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
