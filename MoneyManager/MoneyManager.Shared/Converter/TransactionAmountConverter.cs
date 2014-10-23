using System;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;

namespace MoneyManager.Converter
{
    internal class TransactionAmountConverter : IValueConverter
    {
        private Account selectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
        }

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