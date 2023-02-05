namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.Common.Extensions;
using Core.Common.Helpers;

public class AmountFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var currencyValue = (decimal)value;

        return currencyValue.ToString(format: "C", provider: culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
