using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class PaymentTypeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var paymentTypeEnum = (PaymentType) value;

            switch(paymentTypeEnum)
            {
                case PaymentType.Expense:
                    return Strings.ExpenseLabel;
                case PaymentType.Income:
                    return Strings.IncomeLabel;
                case PaymentType.Transfer:
                    return Strings.TransferLabel;

                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
