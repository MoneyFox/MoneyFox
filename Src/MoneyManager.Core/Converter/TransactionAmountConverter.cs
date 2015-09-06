using System;
using System.Globalization;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Converter
{
    public class TransactionAmountConverter : IMvxValueConverter
    {
        //TODO: remove this and refactor with converter parameter
        private Account selectedAccount => Mvx.Resolve<IRepository<Account>>().Selected;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var transaction = (FinancialTransaction) value;

            if (transaction.Type == (int)TransactionType.Transfer)
            {
                return selectedAccount == transaction.ChargedAccount
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