using System;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Converter
{
    public class AmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value.ToString() == string.Empty)
            {
                return value;
            }

            var valDouble = Double.Parse(value.ToString());
            return (valDouble/100).ToString();
        }
    }
}
