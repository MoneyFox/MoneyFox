namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Resources;

public class PaymentTypeFilterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var filteredPaymentType = (PaymentTypeFilter)Enum.ToObject(enumType: typeof(PaymentTypeFilter), value: value);

        return filteredPaymentType switch
        {
            PaymentTypeFilter.All => Strings.AllLabel,
            PaymentTypeFilter.Expense => Strings.ExpenseLabel,
            PaymentTypeFilter.Income => Strings.IncomeLabel,
            PaymentTypeFilter.Transfer => Strings.TransferLabel,
            _ => string.Empty
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
