using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Views;

namespace MoneyManager.Src
{
    public class TransactionHelper
    {
        public static FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction = value; }
        }

        public static AccountDataAccess AccountData
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public static void GoToAddTransaction(TransactionType transactionType)
        {
            SetDefaultTransaction(transactionType);
            SetDefaultAccount();

            ((Frame)Window.Current.Content).Navigate(typeof(AddTransaction));
        }

        private static void SetDefaultTransaction(TransactionType transactionType)
        {
            ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction
                = new FinancialTransaction
                {
                    Type = (int) transactionType,
                    Currency = "CHF"
                };
        }

        private static void SetDefaultAccount()
        {
            if (AccountData.AllAccounts.Any())
            {
                SelectedTransaction.ChargedAccount = AccountData.AllAccounts.First();
            }
        }

    }
}
