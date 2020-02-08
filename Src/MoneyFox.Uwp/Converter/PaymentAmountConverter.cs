using System;
using Windows.UI.Xaml.Data;
using MoneyFox.Application;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Uwp.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {
        private const string IGNORE_TRANSFER = "IgnoreTransfer";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var payment = value as PaymentViewModel;

            if (payment == null) return string.Empty;

            string param = parameter.ToString();
            string sign;

            if (payment.Type == PaymentType.Transfer)
            {
                string condition;
                condition = payment.ChargedAccountId == payment.CurrentAccountId ? "-" : "+";
                sign = param == IGNORE_TRANSFER ? "-" : condition;
            }
            else
            {
                sign = payment.Type == (int)PaymentType.Expense
                           ? "-"
                           : "+";
            }

            return sign + " " + $"{payment.Amount.ToString("C2", CultureHelper.CurrentCulture)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
