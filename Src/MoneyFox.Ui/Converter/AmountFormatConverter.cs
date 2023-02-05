namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.Common.Extensions;
using Core.Common.Settings;
using Infrastructure.Adapters;

public class AmountFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var settingsAdapter = new SettingsAdapter();
        var currency = settingsAdapter.GetValue(SettingConstants.DEFAULT_CURRENCY_KEY_NAME, new RegionInfo(culture.Name).ISOCurrencySymbol);

        var currencyValue = (decimal)value;
        return currencyValue.FormatCurrency(currency);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
