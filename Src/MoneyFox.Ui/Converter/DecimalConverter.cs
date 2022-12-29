namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.Common.Helpers;

public class DecimalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is decimal decimalValue ? decimalValue.ToString(CultureHelper.CurrentCulture) : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (decimal.TryParse(s: value as string, style: NumberStyles.Currency, provider: CultureHelper.CurrentCulture, result: out var dec))
        {
            return dec;
        }

        return value;
    }
}
