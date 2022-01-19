using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Resources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class PaymentTypeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}