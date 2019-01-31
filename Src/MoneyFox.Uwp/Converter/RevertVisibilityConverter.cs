using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.Converter
{
    public class RevertVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => (Visibility) value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}