namespace MoneyFox.Win.Converter;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;

public class ClickConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language) =>
        ((ItemClickEventArgs)value)?.ClickedItem;

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException();
}