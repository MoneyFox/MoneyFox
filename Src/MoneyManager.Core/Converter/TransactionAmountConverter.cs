using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Converter
{
    public class TransactionAmountConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var transaction = (FinancialTransaction) value;

            if (transaction.Type == (int)TransactionType.Transfer)
            {
                return (Account) parameter == transaction.ChargedAccount
                    ? "-"
                    : "+";
            }

            return transaction.Type == (int)TransactionType.Spending
                ? "-"
                : "+";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}