namespace MoneyFox.Ui.Controls.AccountPicker;

using System.Globalization;
using Core.Common.Extensions;

public class AccountNameConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not AccountPickerViewModel account ? string.Empty : $"{account.Name} ({account.CurrentBalance.FormatCurrency()})";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
