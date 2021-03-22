using MoneyFox.Application;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is decimal)
            {
                return ((decimal)value).ToString(CultureHelper.CurrentLocale);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(decimal.TryParse(value as string, NumberStyles.Currency, CultureHelper.CurrentLocale, out decimal dec))
            {
                return dec;
            }
            return value;
        }
    }
}
