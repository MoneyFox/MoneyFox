using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.ViewModels;
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

        private static AddTransactionViewModel addTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public static void GoToAddTransaction(TransactionType transactionType)
        {
            ServiceLocator.Current.GetInstance<AddTransactionViewModel>().IsEdit = false;
            if (transactionType == TransactionType.Transfer)
            {
                addTransactionView.IsTransfer = true;
            }

            SetDefaultTransaction(transactionType);
            SetDefaultAccount();

            ((Frame)Window.Current.Content).Navigate(typeof(AddTransaction));
        }

        private static void SetDefaultTransaction(TransactionType transactionType)
        {
            SelectedTransaction = new FinancialTransaction
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
