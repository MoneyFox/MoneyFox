namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Domain.Aggregates.AccountAggregate;
using Resources.Strings;

public class PaymentTypeFilterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var filteredPaymentType = (PaymentTypeFilter)Enum.ToObject(enumType: typeof(PaymentTypeFilter), value: value);

        return filteredPaymentType switch
        {
            PaymentTypeFilter.All => Translations.AllLabel,
            PaymentTypeFilter.Expense => Translations.ExpenseLabel,
            PaymentTypeFilter.Income => Translations.IncomeLabel,
            PaymentTypeFilter.Transfer => Translations.TransferLabel,
            _ => string.Empty
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
