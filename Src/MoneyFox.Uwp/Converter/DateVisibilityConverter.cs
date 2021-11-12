using System;
using Windows.UI.Xaml.Data;

#nullable enable
namespace MoneyFox.Uwp.Converter
{
    public class DateVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => (DateTime)value != new DateTime();

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotSupportedException();
    }
}