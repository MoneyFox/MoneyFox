using System;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is decimal)
            {
                return value.ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(decimal.TryParse(value as string, out decimal dec))
            {
                return dec;
            }
            return value;
        }
    }
}
