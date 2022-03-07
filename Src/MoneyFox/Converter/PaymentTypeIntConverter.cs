namespace MoneyFox.Converter
{
    using Core.Aggregates.Payments;
    using Core.Resources;
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class PaymentTypeIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // A value of -1 is expected from lists that offer an option for not selecting a specific payment type.
            if(value is int val && val == -1)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
