using MoneyFox.Application;
using MoneyFox.Domain;
using MoneyFox.Uwp.ViewModels;
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

            if(payment.Type == PaymentType.Transfer)
            {
                sign = payment.ChargedAccountId == payment.CurrentAccountId
                            ? "-"
                            : "+";
            }
            else
            {
                sign = payment.Type == (int) PaymentType.Expense
                       ? "-"
                       : "+";
            }

            return $"{sign} {payment.Amount.ToString("C2", CultureHelper.CurrentCulture)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
