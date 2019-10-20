using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using MoneyFox.Presentation.ConverterLogic;

namespace MoneyFox.Uwp.Converter
{
    public class TodayMarkerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime dateTime = System.Convert.ToDateTime(value);

            return TodayMarkerVisibilityConverterLogic.ShowMarker(dateTime) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
