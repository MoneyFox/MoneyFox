using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Business.Converter
{
    public class NoteVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            return string.IsNullOrEmpty(value.ToString())
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}