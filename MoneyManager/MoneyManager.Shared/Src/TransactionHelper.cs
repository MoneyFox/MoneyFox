using System;
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

        public static void GoToTransaction(string transactionType)
        {
            ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction 
                = new FinancialTransaction
            {
                Type = (int)Enum.Parse(typeof(TransactionType), transactionType)
            };
            ((Frame)Window.Current.Content).Navigate(typeof(AddTransaction));
        }
    }
}
