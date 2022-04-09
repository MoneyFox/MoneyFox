namespace MoneyFox.Win.Converter;

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

public class RevertVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (Visibility)value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
