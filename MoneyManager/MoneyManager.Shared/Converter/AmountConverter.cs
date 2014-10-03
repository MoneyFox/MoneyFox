using System;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Converter
{
    public class AmountConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
