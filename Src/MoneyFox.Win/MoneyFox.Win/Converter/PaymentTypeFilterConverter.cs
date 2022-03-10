namespace MoneyFox.Win.Converter;

using Core.Aggregates.Payments;
using Core.Resources;
using Microsoft.UI.Xaml.Data;
using System;

public class PaymentTypeFilterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var filteredPaymentType = (PaymentTypeFilter)Enum.ToObject(typeof(PaymentTypeFilter), value);

        return filteredPaymentType switch
        {
            PaymentTypeFilter.All => Strings.AllLabel,
            PaymentTypeFilter.Expense => Strings.ExpenseLabel,
            PaymentTypeFilter.Income => Strings.IncomeLabel,
            PaymentTypeFilter.Transfer => Strings.TransferLabel,
            _ => string.Empty
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException();
}
