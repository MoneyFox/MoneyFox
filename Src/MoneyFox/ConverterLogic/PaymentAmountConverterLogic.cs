namespace MoneyFox.ConverterLogic
{
    using Core._Pending_;
    using Core.Aggregates.Payments;
    using Core.Common;
    using ViewModels.Payments;

    public static class PaymentAmountConverterLogic
    {
        public static string GetAmountSign(PaymentViewModel paymentViewModel)
        {
            string sign;

            sign = paymentViewModel.Type == PaymentType.Transfer
                ? GetSignForTransfer(paymentViewModel)
                : GetSignForNonTransfer(paymentViewModel);

            return $"{sign} {paymentViewModel.Amount.ToString("C2", CultureHelper.CurrentCulture)}";
        }

        private static string GetSignForTransfer(PaymentViewModel payment)
            => payment.ChargedAccountId == payment.CurrentAccountId
                ? "-"
                : "+";

        private static string GetSignForNonTransfer(PaymentViewModel payment)
            => payment.Type == (int)PaymentType.Expense
                ? "-"
                : "+";
    }
}