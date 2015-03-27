using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Converter {
    public class NoteVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value == null) {
                return Visibility.Collapsed;
            }

            return String.IsNullOrEmpty(value.ToString())
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}