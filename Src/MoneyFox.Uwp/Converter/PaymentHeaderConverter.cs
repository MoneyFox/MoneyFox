using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using System;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.Converter
{
    public class PaymentHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var pType = (PaymentType) value;
            switch(pType)
            {
                default:
                case PaymentType.Income:
                    return Strings.IncomeHeader;
                case PaymentType.Expense:
                    return Strings.ExpenseHeader;
                case PaymentType.Transfer:
                    return Strings.TransferHeader;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
