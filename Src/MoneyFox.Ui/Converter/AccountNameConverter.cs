namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.Common.Helpers;
using Views.Accounts;

public class AccountNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not AccountViewModel account
            ? string.Empty
            : $"{account.Name} ({account.CurrentBalance.ToString(format: "C", provider: CultureHelper.CurrentCulture)})";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
