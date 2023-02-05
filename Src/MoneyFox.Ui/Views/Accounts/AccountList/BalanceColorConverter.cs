namespace MoneyFox.Ui.Views.Accounts.AccountList;

using System.Globalization;

public class BalanceColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var styleKey = value is decimal d && d < 0 ? "BalanceLabelNegative" : "BalanceLabelPositive";

        return Application.Current!.Resources.TryGetValue(key: styleKey, value: out var styleValue)
            ? styleValue
            : throw new InvalidOperationException($"Failed to find the style '{styleKey}' in the resource dictionary.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
