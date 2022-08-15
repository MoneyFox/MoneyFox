namespace MoneyFox.Win.Converter;

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

public class BalanceColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, string language)
    {
        var styleKey = value is decimal d && d < 0 ? "NegativeBalanceColorBrush" : "AppForegroundPrimaryBrush";

        return Application.Current.Resources.TryGetValue(key: styleKey, value: out var styleValue)
            ? styleValue
            : throw new InvalidOperationException($"Failed to find the style '{styleKey}' in the resource dictionary.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
