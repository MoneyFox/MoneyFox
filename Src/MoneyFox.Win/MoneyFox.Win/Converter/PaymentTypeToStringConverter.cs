namespace MoneyFox.Win.Converter;

using System;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Resources;
using Microsoft.UI.Xaml.Data;

public class PaymentTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var paymentType = (PaymentType)Enum.ToObject(enumType: typeof(PaymentType), value: value);

        return paymentType switch
        {
            PaymentType.Expense => Strings.ExpenseLabel,
            PaymentType.Income => Strings.IncomeLabel,
            PaymentType.Transfer => Strings.TransferLabel,
            _ => string.Empty
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
