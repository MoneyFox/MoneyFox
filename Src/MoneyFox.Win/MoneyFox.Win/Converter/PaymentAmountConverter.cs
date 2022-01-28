using Microsoft.UI.Xaml.Data;
using MoneyFox.Win.ConverterLogic;
using MoneyFox.Win.ViewModels.Payments;
using System;

namespace MoneyFox.Win.Converter
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

            return PaymentAmountConverterLogic.GetAmountSign(payment);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotSupportedException();
    }
}