namespace MoneyFox.Win.Converter;

using ConverterLogic;
using Microsoft.UI.Xaml.Data;
using System;
using ViewModels.Payments;

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