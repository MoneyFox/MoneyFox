namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.Common.Extensions;
using Core.Common.Settings;
using Infrastructure.Adapters;
using Views.Accounts;

public class AccountNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var settingsAdapter = new SettingsAdapter();
        var currency = settingsAdapter.GetValue(SettingConstants.DEFAULT_CURRENCY_KEY_NAME, new RegionInfo(culture.Name).ISOCurrencySymbol);

        return value is not AccountViewModel account
            ? string.Empty
            : $"{account.Name} ({account.CurrentBalance.FormatCurrency(currency)})";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
