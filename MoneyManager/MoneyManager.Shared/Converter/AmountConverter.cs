using System;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Converter
{
    internal class AmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToDouble(value).ToString("F2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}