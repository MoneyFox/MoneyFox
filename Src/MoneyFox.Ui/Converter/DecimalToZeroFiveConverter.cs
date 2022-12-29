namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.Common.Helpers;

public class DecimalToZeroFiveConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is decimal decimalValue ? RoundDecimalToFive(decimalValue).ToString(format: "F2", provider: culture) : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (decimal.TryParse(s: value as string, style: NumberStyles.Currency, provider: CultureHelper.CurrentCulture, result: out var dec))
        {
            return dec;
        }

        return value;
    }

    private static decimal RoundDecimalToFive(decimal decimalValue)
    {
        return (int)Math.Round(d: decimalValue * 20) / 20m;
    }
}
