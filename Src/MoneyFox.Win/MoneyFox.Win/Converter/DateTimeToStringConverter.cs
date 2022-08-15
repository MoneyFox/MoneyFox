namespace MoneyFox.Win.Converter;

using System;
using Core.Common.Helpers;
using Microsoft.UI.Xaml.Data;

public class DateTimeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return ((DateTime)value).ToString(format: "d", provider: CultureHelper.CurrentCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
