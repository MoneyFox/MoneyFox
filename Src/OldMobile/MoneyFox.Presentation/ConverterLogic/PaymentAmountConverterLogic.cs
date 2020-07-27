using MoneyFox.Application;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.ConverterLogic
{
    public static class PaymentAmountConverterLogic
    {
        private const string IGNORE_TRANSFER = "IgnoreTransfer";

        public static string GetFormattedAmountString(PaymentViewModel payment, string parameter)
        {
            if(payment == null)
                return string.Empty;

            string param = parameter;
            string sign;

            if(payment.Type == PaymentType.Transfer)
            {
                string condition;
                condition = payment.ChargedAccountId == payment.CurrentAccountId
                            ? "-" : "+";
                sign = param == IGNORE_TRANSFER
                       ? "-" : condition;
            }
            else
            {
                sign = payment.Type == (int) PaymentType.Expense
                       ? "-"
                       : "+";
            }

            return $"{sign} {payment.Amount.ToString("C2", CultureHelper.CurrentCulture)}";
        }
    }
}
