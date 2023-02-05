namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.Common.Extensions;
using Core.Common.Settings;
using Domain.Aggregates.AccountAggregate;
using Infrastructure.Adapters;
using Views.Payments;

public class PaymentAmountConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is PaymentViewModel model ? GetAmountSign(model) : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private static string GetAmountSign(PaymentViewModel paymentViewModel)
    {
        var settingsAdapter = new SettingsAdapter();
        var currency = settingsAdapter.GetValue(key: SettingConstants.DEFAULT_CURRENCY_KEY_NAME, defaultValue: RegionInfo.CurrentRegion.ISOCurrencySymbol);
        var sign = paymentViewModel.Type == PaymentType.Transfer ? GetSignForTransfer(paymentViewModel) : GetSignForNonTransfer(paymentViewModel);

        return $"{sign} {paymentViewModel.Amount.FormatCurrency(currency)}";
    }

    private static string GetSignForTransfer(PaymentViewModel payment)
    {
        return payment.ChargedAccountId == payment.CurrentAccountId ? "-" : "+";
    }

    private static string GetSignForNonTransfer(PaymentViewModel payment)
    {
        return payment.Type == (int)PaymentType.Expense ? "-" : "+";
    }
}
