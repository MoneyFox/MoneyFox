using System;
using Windows.UI.Xaml.Data;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Application.Common.ConverterLogic;

namespace MoneyFox.Uwp.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
            => PaymentAmountConverterLogic.GetFormattedAmountString(value as PaymentViewModel, parameter as string);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotSupportedException();
    }
}
