using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using MoneyManager.ViewModels;

namespace MoneyManager.Business
{
    internal class TransactionLogic
    {
        #region Properties
        private static FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction = value; }
        }

        private static AccountDataAccess accountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private static TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private static AddTransactionViewModel addTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        #endregion

        public static void GoToAddTransaction(TransactionType transactionType)
        {
            addTransactionView.IsEdit = false;
            addTransactionView.IsEndless = true;
            addTransactionView.IsTransfer = transactionType == TransactionType.Transfer;
            SetDefaultTransaction(transactionType);
            SetDefaultAccount();
        }

        public static void PrepareEdit(FinancialTransaction transaction)
        {
            addTransactionView.IsEdit = true;
            if (transaction.ReccuringTransactionId.HasValue)
            {
                addTransactionView.IsEndless = transaction.RecurringTransaction.IsEndless;
                addTransactionView.Recurrence = transaction.RecurringTransaction.Recurrence;
            }
            addTransactionView.SelectedTransaction = transaction;
        }

        public static void DeleteTransaction(FinancialTransaction transaction)
        {
            transactionData.Delete(transaction);
            AccountLogic.RemoveTransactionAmount(SelectedTransaction);
        }

        public static void DeleteAssociatedTransactionsFromDatabase(int accountId)
        {
            var transactions = transactionData.LoadList();

            foreach (var transaction in transactions)
            {
                transactionData.Delete(transaction);
            }
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
            if (accountDataAccess.AllAccounts.Any())
            {
                SelectedTransaction.ChargedAccount = accountDataAccess.AllAccounts.First();
            }
        }

        public void ClearTransaction()
        {
            IEnumerable<FinancialTransaction> transactions = transactionData.GetUnclearedTransactions();
            foreach (FinancialTransaction transaction in transactions)
            {
                AccountLogic.AddTransactionAmount(transaction);
            }
        }

    }
}