using System;
using Windows.UI.Xaml.Data;
using MoneyFox.Application.Common.ConverterLogic;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Uwp.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return PaymentAmountConverterLogic.GetFormattedAmountString(value as PaymentViewModel, parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
