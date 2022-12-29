namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Resources;

public class PaymentTypeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var paymentType = (PaymentType)Enum.ToObject(enumType: typeof(PaymentType), value: value);

        return paymentType switch
        {
            PaymentType.Expense => Translations.ExpenseLabel,
            PaymentType.Income => Translations.IncomeLabel,
            PaymentType.Transfer => Translations.TransferLabel,
            _ => string.Empty
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
