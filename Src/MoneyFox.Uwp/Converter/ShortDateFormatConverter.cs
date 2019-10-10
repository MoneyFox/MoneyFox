using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.Converter
{
    public class ShortDateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return ((DateTime)value).ToString("d", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
