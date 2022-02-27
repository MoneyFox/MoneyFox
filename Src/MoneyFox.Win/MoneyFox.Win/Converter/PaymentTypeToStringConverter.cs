namespace MoneyFox.Win.Converter;

using Core.Aggregates.Payments;
using Core.Resources;
using Microsoft.UI.Xaml.Data;
using System;

public class PaymentTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var paymentType = (PaymentType)Enum.ToObject(typeof(PaymentType), value);

        return paymentType switch
        {
            PaymentType.Expense => Strings.ExpenseLabel,
            PaymentType.Income => Strings.IncomeLabel,
            PaymentType.Transfer => Strings.TransferLabel,
            _ => string.Empty
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException();
}