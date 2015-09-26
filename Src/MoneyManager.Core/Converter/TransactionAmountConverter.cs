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
            string sign;

            if (transaction.Type == (int) TransactionType.Transfer)
            {
                sign = (Account) parameter == transaction.ChargedAccount
                    ? "-"
                    : "+";
            }
            else
            {
                sign = transaction.Type == (int) TransactionType.Spending
                    ? "-"
                    : "+";
            }

            return sign + " " + $"{transaction.Amount:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}