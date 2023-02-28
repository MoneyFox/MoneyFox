namespace MoneyFox.Ui.Views.Payments.PaymentList;

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
        return value is PaymentListItemViewModel model ? GetAmountSign(model) : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private static string GetAmountSign(PaymentListItemViewModel paymentViewModel)
    {
        var settingsAdapter = new SettingsAdapter();
        var currency = settingsAdapter.GetValue(key: SettingConstants.DEFAULT_CURRENCY_KEY_NAME, defaultValue: RegionInfo.CurrentRegion.ISOCurrencySymbol);
        var sign = paymentViewModel.Type == PaymentType.Transfer ? GetSignForTransfer(paymentViewModel) : GetSignForNonTransfer(paymentViewModel);

        return $"{sign} {paymentViewModel.Amount.FormatCurrency(currency)}";
    }

    private static string GetSignForTransfer(PaymentListItemViewModel payment)
    {
        return payment.ChargedAccountId == payment.CurrentAccountId ? "-" : "+";
    }

    private static string GetSignForNonTransfer(PaymentListItemViewModel payment)
    {
        return payment.Type == (int)PaymentType.Expense ? "-" : "+";
    }
}
