using System;
using Windows.UI.Xaml.Data;
using MoneyFox.Application;

namespace MoneyFox.Uwp.Converter
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var date = System.Convert.ToDateTime(value, CultureHelper.CurrentCulture);
                return new DateTimeOffset(date);
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTimeOffset.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var dto = (DateTimeOffset) value;

                return dto.DateTime;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
    }
}
