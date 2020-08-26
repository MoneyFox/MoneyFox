using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using System;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.Converter
{
    public class PaymentTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var paymentType = (PaymentType)Enum.ToObject(typeof(PaymentType), value);

            switch(paymentType)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
