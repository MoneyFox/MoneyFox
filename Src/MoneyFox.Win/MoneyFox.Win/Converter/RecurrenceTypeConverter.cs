namespace MoneyFox.Win.Converter;

using System;
using ConverterLogic;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Microsoft.UI.Xaml.Data;

public class RecurrenceTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return RecurrenceTypeConverterLogic.GetStringForPaymentRecurrence((PaymentRecurrence)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
