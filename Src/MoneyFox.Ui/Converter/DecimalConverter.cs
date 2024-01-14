namespace MoneyFox.Ui.Converter;

using System.Globalization;

public class DecimalConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is decimal decimalValue ? decimalValue.ToString(culture) : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return decimal.TryParse(s: value as string, style: NumberStyles.Currency, provider: culture, result: out var dec) ? dec : value;
    }
}
