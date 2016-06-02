using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Windows.Converter
{
    public class SelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ((SelectionChangedEventArgs) value)?.AddedItems.FirstOrDefault();

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ClickConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ((ItemClickEventArgs) value)?.ClickedItem;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}