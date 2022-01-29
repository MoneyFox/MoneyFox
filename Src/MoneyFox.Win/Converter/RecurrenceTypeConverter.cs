using Microsoft.UI.Xaml.Data;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Win.ConverterLogic;
using System;

namespace MoneyFox.Win.Converter
{
    public class RecurrenceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            RecurrenceTypeConverterLogic.GetStringForPaymentRecurrence((PaymentRecurrence)value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotSupportedException();
    }
}