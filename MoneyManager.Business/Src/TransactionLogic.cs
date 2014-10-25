using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MoneyManager.Business.Src
{
    internal class TransactionLogic
    {
        #region Properties

        private static AccountDataAccess accountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private static TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private static RecurringTransactionDataAccess recurringTransactionData
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        private static AddTransactionViewModel addTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        private static TotalBalanceViewModel totalBalanceView
        {
            get { return ServiceLocator.Current.GetInstance<TotalBalanceViewModel>(); }
        }

        #endregion Properties

        public static void SaveTransaction(FinancialTransaction transaction, bool skipRecurring = false)
        {
            AccountLogic.AddTransactionAmount(transaction);
            if (!skipRecurring && transaction.IsRecurring)
            {
                var recurringTransaction = RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);

                recurringTransactionData.Save(transaction, recurringTransaction);
            }

            AccountLogic.RemoveTransactionAmount(transaction);
            totalBalanceView.UpdateBalance();

            transactionData.Save(transaction);
        }

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
            AccountLogic.RemoveTransactionAmount(transactionData.SelectedTransaction);
            AccountLogic.RefreshRelatedTransactions(transaction);
            totalBalanceView.UpdateBalance();

            CheckForRecurringTransaction(transaction,
                () => recurringTransactionData.Delete(transaction.ReccuringTransactionId.Value));
        }

        public static void DeleteAssociatedTransactionsFromDatabase(int accountId)
        {
            foreach (
                FinancialTransaction transaction in
                    transactionData.AllTransactions.Where(x => x.ChargedAccountId == accountId))
            {
                transactionData.Delete(transaction);
            }
        }

        public static async void UpdateTransaction(FinancialTransaction transaction)
        {
            CheckIfRecurringWasRemoved(transaction);
            AccountLogic.AddTransactionAmount(transaction);
            transactionData.Update(transaction);

            var recurringTransaction = RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);

            await CheckForRecurringTransaction(transaction, () => recurringTransactionData.Update(transaction, recurringTransaction));
        }

        private static async Task CheckForRecurringTransaction(FinancialTransaction transaction,
            Action recurringTransactionAction)
        {
            if (!transaction.IsRecurring) return;

            var dialog =
                new MessageDialog(Translation.GetTranslation("ChangeSubsequentTransactionsMessage"),
                    Translation.GetTranslation("ChangeSubsequentTransactionsTitle"));

            dialog.Commands.Add(new UICommand(Translation.GetTranslation("RecurringLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("JustThisLabel")));

            dialog.DefaultCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();

            if (result.Label == Translation.GetTranslation("RecurringLabel"))
            {
                recurringTransactionAction();
            }
        }

        private static void CheckIfRecurringWasRemoved(FinancialTransaction transaction)
        {
            if (!transaction.IsRecurring && transaction.ReccuringTransactionId != null)
            {
                recurringTransactionData.Delete(transaction.ReccuringTransactionId.Value);
            }
        }

        private static void SetDefaultTransaction(TransactionType transactionType)
        {
            transactionData.SelectedTransaction = new FinancialTransaction
            {
                Type = (int)transactionType,
            };
        }

        private static void SetDefaultAccount()
        {
            if (accountDataAccess.AllAccounts.Any())
            {
                transactionData.SelectedTransaction.ChargedAccount = accountDataAccess.AllAccounts.First();
            }
        }

        public static void ClearTransactions()
        {
            IEnumerable<FinancialTransaction> transactions = transactionData.GetUnclearedTransactions();
            foreach (FinancialTransaction transaction in transactions)
            {
                AccountLogic.AddTransactionAmount(transaction);
            }
        }
    }
}