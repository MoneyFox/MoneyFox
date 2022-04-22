namespace MoneyFox.Converter
{

    using System;
    using System.Globalization;
    using Core.Aggregates.AccountAggregate;
    using Core.Resources;
    using Xamarin.Forms;

    public class PaymentTypeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
