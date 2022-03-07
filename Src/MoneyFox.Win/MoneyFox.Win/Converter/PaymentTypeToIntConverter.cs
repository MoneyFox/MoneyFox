namespace MoneyFox.Win.Converter;

using Core.Aggregates.Payments;
using Core.Resources;
using Microsoft.UI.Xaml.Data;
using System;

public class PaymentTypeToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // A value of -1 is expected from lists that offer an option for not selecting a specific payment type.
        if(value is int && (int)value == -1)
        {
            return Strings.AllLabel;
        }

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
