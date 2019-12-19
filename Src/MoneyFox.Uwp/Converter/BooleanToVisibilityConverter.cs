using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.Converter
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter != null && parameter.ToString() == "revert") return (bool) value ? Visibility.Collapsed : Visibility.Visible;

            return (bool) value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
