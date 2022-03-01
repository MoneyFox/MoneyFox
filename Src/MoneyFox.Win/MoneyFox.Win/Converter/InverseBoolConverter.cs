namespace MoneyFox.Win.Converter;

using Microsoft.UI.Xaml.Data;
using System;

public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) => !(bool)value;
    public object ConvertBack(object value, Type targetType, object parameter, string language) => !(bool)value;
}