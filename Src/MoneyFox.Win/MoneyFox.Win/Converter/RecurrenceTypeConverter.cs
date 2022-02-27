namespace MoneyFox.Win.Converter;

using ConverterLogic;
using Core.Aggregates.Payments;
using Microsoft.UI.Xaml.Data;
using System;

public class RecurrenceTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        RecurrenceTypeConverterLogic.GetStringForPaymentRecurrence((PaymentRecurrence)value);

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException();
}