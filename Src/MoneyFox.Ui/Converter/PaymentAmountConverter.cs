namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Common.ConverterLogic;
using Core.Common.Helpers;
using Domain.Aggregates.AccountAggregate;
using Views.Payments;

public class PaymentAmountConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is PaymentViewModel model ? GetAmountSign(model) : string.Empty;
    }

    private static string GetAmountSign(PaymentViewModel paymentViewModel)
    {
        string sign;
        sign = paymentViewModel.Type == PaymentType.Transfer ? GetSignForTransfer(paymentViewModel) : GetSignForNonTransfer(paymentViewModel);

        return $"{sign} {paymentViewModel.Amount.ToString(format: "C2", provider: CultureHelper.CurrentCulture)}";
    }

    private static string GetSignForTransfer(PaymentViewModel payment)
    {
        return payment.ChargedAccountId == payment.CurrentAccountId ? "-" : "+";
    }

    private static string GetSignForNonTransfer(PaymentViewModel payment)
    {
        return payment.Type == (int)PaymentType.Expense ? "-" : "+";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
