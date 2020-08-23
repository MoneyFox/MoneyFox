using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Categories;
using System.Globalization;
using Xamarin.Forms;
using System;
using MoneyFox.Application;
using MoneyFox.Domain;
using MoneyFox.ViewModels.Payments;

namespace MoneyFox.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {
        private const string IGNORE_TRANSFER = "IgnoreTransfer";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var payment = value as PaymentViewModel;

            if(payment == null)
                return string.Empty;

            string param = parameter == null
                                ? ""
                                : parameter.ToString();
            string sign;

            if(payment.Type == PaymentType.Transfer)
            {
                string condition;
                condition = payment.ChargedAccountId == payment.CurrentAccountId
                            ? "-" : "+";
                sign = param == IGNORE_TRANSFER
                       ? "-"
                       : condition;
            }
            else
            {
                sign = payment.Type == (int)PaymentType.Expense
                       ? "-"
                       : "+";
            }

            return $"{sign} {payment.Amount.ToString("C2", CultureHelper.CurrentCulture)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
