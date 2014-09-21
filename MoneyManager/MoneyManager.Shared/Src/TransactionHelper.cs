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
        public FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction = value; }
        }

        public static void GoToAddTransaction(TransactionType transactionType)
        {
            ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction 
                = new FinancialTransaction
            {
                Type = (int)transactionType,
                Currency = "CHF"
            };
            ((Frame)Window.Current.Content).Navigate(typeof(AddTransaction));
        }
    }
}
