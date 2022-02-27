namespace MoneyFox.Win.Converter;

using Core._Pending_;
using Microsoft.UI.Xaml.Data;
using System;

public class DateTimeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
        => ((DateTime)value).ToString("d", CultureHelper.CurrentCulture);

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException();
}