namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.Common.Extensions;
using Core.Common.Settings;
using Infrastructure.Adapters;
using MoneyFox.Ui.Views.Accounts.AccountModification;

public class AccountNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var settingsAdapter = new SettingsAdapter();
        var currency = settingsAdapter.GetValue(key: SettingConstants.DEFAULT_CURRENCY_KEY_NAME, defaultValue: new RegionInfo(culture.Name).ISOCurrencySymbol);

        return value is not AccountViewModel account ? string.Empty : $"{account.Name} ({account.CurrentBalance.FormatCurrency(currency)})";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
