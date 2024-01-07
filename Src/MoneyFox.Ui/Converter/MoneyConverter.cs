namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Domain;

internal sealed class MoneyConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Money money)
        {
            return $"{money.Amount} {money.Currency.AlphaIsoCode}";
        }

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
