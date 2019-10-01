using System;
using Windows.UI.Xaml.Data;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Uwp.Converter
{
    public class NativePaymentAmountConverter : IValueConverter
    {
        private const string IGNORE_TRANSFER = "IgnoreTransfer";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return string.Empty;

            var payment = (PaymentViewModel)value;
            var param = parameter as string;
            string sign;

            if (payment.Type == PaymentType.Transfer)
            {
                if (param == IGNORE_TRANSFER)
                {
                    sign = "-";
                } else
                {
                    sign = payment.ChargedAccountId == payment.CurrentAccountId
                        ? "-"
                        : "+";
                }
            } else
            {
                sign = payment.Type == (int)PaymentType.Expense
                    ? "-"
                    : "+";
            }

            return sign + " " + $"{payment.Amount:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
