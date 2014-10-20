using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using System;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Converter
{
    public class TransactionAmountConverter : IValueConverter
    {
        private Account selectedAccount
        {
            get{ return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
        }
        
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if((int)value == (int)TransactionType.Transfer)
            {
                var transaction = parameter as FinancialTransaction;
                
                return selectedAccount == transaction.ChargedAccount
                    ? "-"
                    : "+";
            }
            
            return (int)value == (int)TransactionType.Spending
                ? "-"
                : "+";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
