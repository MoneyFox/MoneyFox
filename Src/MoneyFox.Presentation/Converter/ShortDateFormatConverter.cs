using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Converter
{
    public class ShortDateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime) value).ToString("d", CultureHelper.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
