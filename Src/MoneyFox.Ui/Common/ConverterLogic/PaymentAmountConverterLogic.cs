namespace MoneyFox.Ui.Common.ConverterLogic;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Common.Helpers;
using Views.Payments;

public static class PaymentAmountConverterLogic
{
    public static string GetAmountSign(PaymentViewModel paymentViewModel)
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
}
