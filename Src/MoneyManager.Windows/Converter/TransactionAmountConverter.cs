using System;
using Windows.UI.Xaml.Data;
using Cirrious.CrossCore;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Windows.Converter
{
    public class TransactionAmountConverter : IValueConverter
    {
        //TODO: remove this and refactor with converter parameter
        private Account selectedAccount => Mvx.Resolve<IRepository<Account>>().Selected;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var transaction = value as FinancialTransaction;

            if (transaction.Type == (int) TransactionType.Transfer)
            {
                return selectedAccount == transaction.ChargedAccount
                    ? "-"
                    : "+";
            }

            return transaction.Type == (int) TransactionType.Spending
                ? "-"
                : "+";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}