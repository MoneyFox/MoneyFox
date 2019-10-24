using System;
using System.Globalization;
using MoneyFox.Presentation.ConverterLogic;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Converter
{
    public class TodayMarkerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = System.Convert.ToDateTime(value);

            return TodayMarkerVisibilityConverterLogic.ShowMarker(dateTime);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
