using MoneyFox.Domain;
using MoneyFox.Ui.Shared;
using System;
using Windows.UI.Xaml.Data;

#nullable enable
namespace MoneyFox.Uwp.Converter
{
    public class RecurrenceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => RecurrenceTypeConverterLogic.GetStringForPaymentRecurrence((PaymentRecurrence)value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException();
    }
}
