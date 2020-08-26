using MoneyFox.Application;
using System;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.Converter
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => ((DateTime)value).ToString("d", CultureHelper.CurrentCulture);
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
