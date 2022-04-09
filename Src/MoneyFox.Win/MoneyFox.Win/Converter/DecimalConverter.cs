namespace MoneyFox.Win.Converter;

using System;
using Microsoft.UI.Xaml.Data;

public class DecimalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        decimal.TryParse(s: (string)value, result: out var result);

        return result;
    }
}
