using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.ViewModels;

namespace MoneyManager.Src
{
    internal class TransactionHelper
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
            addTransactionView.IsEdit = false;
            addTransactionView.IsEndless = true;
            addTransactionView.IsTransfer = transactionType == TransactionType.Transfer;
            SetDefaultTransaction(transactionType);
            SetDefaultAccount();

            ((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
        }

        private static void SetDefaultTransaction(TransactionType transactionType)
        {
            SelectedTransaction = new FinancialTransaction
            {
                Type = (int) transactionType,
            };
        }

        private static void SetDefaultAccount()
        {
            if (AccountData.AllAccounts.Any())
            {
                SelectedTransaction.ChargedAccount = AccountData.AllAccounts.First();
            }
        }

        public static void GoToEdit(FinancialTransaction transaction)
        {
            addTransactionView.IsEdit = true;
            if (transaction.ReccuringTransactionId.HasValue)
            {
                addTransactionView.IsEndless = transaction.RecurringTransaction.IsEndless;
                addTransactionView.Recurrence = transaction.RecurringTransaction.Recurrence;
            }
            addTransactionView.SelectedTransaction = transaction;

            ((Frame) Window.Current.Content).Navigate(typeof (AddTransaction));
        }
    }
}