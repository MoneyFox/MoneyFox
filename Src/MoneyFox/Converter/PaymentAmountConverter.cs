using MoneyFox.Ui.Shared.ConverterLogic;
using MoneyFox.Ui.Shared.ViewModels.Payments;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var payment = (PaymentViewModel)value;

            if(payment == null)
            {
                return string.Empty;
            }

            return PaymentAmountConverterLogic.GetAmountSign(payment);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
