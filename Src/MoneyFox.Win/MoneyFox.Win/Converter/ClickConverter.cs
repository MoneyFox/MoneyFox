namespace MoneyFox.Win.Converter;

using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

public class ClickConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        return ((ItemClickEventArgs)value)?.ClickedItem;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
