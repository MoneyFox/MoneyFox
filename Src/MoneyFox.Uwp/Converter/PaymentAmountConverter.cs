using MoneyFox.Application;
using MoneyFox.Domain;
using MoneyFox.Ui.Shared.ViewModels.Payments;
using System;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var payment = (PaymentViewModel)value;

            if(payment == null)
            {
                return string.Empty;
            }

            string sign;

            sign = payment.Type == PaymentType.Transfer
                ? GetSignForTransfer(payment)
                : GetSignForNonTransfer(payment);

            return $"{sign} {payment.Amount.ToString("C2", CultureHelper.CurrentCulture)}";
        }

        private string GetSignForTransfer(PaymentViewModel payment)
            => payment.ChargedAccountId == payment.CurrentAccountId
                       ? "-"
                       : "+";

        private string GetSignForNonTransfer(PaymentViewModel payment)
            => payment.Type == (int)PaymentType.Expense
                       ? "-"
                       : "+";

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException();
    }
}
