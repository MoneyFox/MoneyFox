namespace MoneyFox.Win.Converter;

using System;
using Core.Aggregates.Payments;
using Core.Resources;
using Microsoft.UI.Xaml.Data;

public class PaymentHeaderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var pType = (PaymentType)value;

        return pType switch
        {
            PaymentType.Expense => Strings.ExpenseHeader,
            PaymentType.Transfer => Strings.TransferHeader,
            _ => Strings.IncomeHeader
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
