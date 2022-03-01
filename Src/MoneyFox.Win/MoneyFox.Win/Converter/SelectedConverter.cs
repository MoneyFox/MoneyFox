namespace MoneyFox.Win.Converter;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Linq;

public class SelectedConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language) =>
        ((SelectionChangedEventArgs)value)?.AddedItems.FirstOrDefault();

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException();
}