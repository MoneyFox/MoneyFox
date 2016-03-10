using System;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Windows.Converter
{
    public class AmountFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"{value:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}