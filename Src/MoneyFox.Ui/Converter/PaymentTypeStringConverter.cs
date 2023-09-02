namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Domain.Aggregates.AccountAggregate;
using Resources.Strings;

public class PaymentTypeStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var paymentType = (PaymentType)Enum.ToObject(enumType: typeof(PaymentType), value: value);

        return paymentType switch
        {
            PaymentType.Expense => Translations.ExpenseLabel,
            PaymentType.Income => Translations.IncomeLabel,
            PaymentType.Transfer => Translations.TransferLabel,
            _ => string.Empty
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
