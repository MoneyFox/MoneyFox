using Microsoft.UI.Xaml.Data;
using MoneyFox.Core._Pending_;
using System;

namespace MoneyFox.Win.Converter
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ((DateTime)value).ToString("d", CultureHelper.CurrentCulture);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotSupportedException();
    }
}