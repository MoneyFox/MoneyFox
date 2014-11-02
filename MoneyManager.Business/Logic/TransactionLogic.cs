using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MoneyManager.Business.Logic
{
    public class TransactionLogic
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

        private static FinancialTransaction selectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction = value; }
        }

        private static RecurringTransactionDataAccess recurringTransactionData
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        private static AddTransactionViewModel addTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        #endregion Properties

        public static void SaveTransaction(FinancialTransaction transaction, bool refreshRelatedList = false, bool skipRecurring = false)
        {
            if (transaction.IsRecurring && !skipRecurring)
            {
                var recurringTransaction = RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);
                recurringTransactionData.Save(transaction, recurringTransaction);
                transaction.RecurringTransaction = recurringTransaction;
            }

            AccountLogic.RemoveTransactionAmount(transaction);

            transactionData.Save(transaction);

            if (refreshRelatedList)
            {
                ServiceLocator.Current.GetInstance<TransactionListViewModel>()
                    .SetRelatedTransactions(accountDataAccess.SelectedAccount.Id);
            }
            AccountLogic.AddTransactionAmount(transaction);
        }

        public static void GoToAddTransaction(TransactionType transactionType, bool refreshRelatedList = false)
        {
            addTransactionView.IsEdit = false;
            addTransactionView.IsEndless = true;
            addTransactionView.RefreshRealtedList = refreshRelatedList;
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

        public async static void DeleteTransaction(FinancialTransaction transaction)
        {
            if (await Utilities.IsDeletionConfirmed())
            {
                await CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionLogic.Delete(transaction.RecurringTransaction));

                transactionData.Delete(transaction);

                AccountLogic.RemoveTransactionAmount(transaction);
                AccountLogic.RefreshRelatedTransactions(transaction);
            }
        }

        public static void DeleteAssociatedTransactionsFromDatabase(int accountId)
        {
            foreach (
                var transaction in
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

            RecurringTransaction recurringTransaction =
                RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);

            await
                CheckForRecurringTransaction(transaction,
                    () => recurringTransactionData.Update(transaction, recurringTransaction));
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

            var result = await dialog.ShowAsync();

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
            selectedTransaction = new FinancialTransaction
            {
                Type = (int)transactionType,
                IsExchangeModeActive = false
            };
        }

        private static void SetDefaultAccount()
        {
            if (accountDataAccess.SelectedAccount != null)
            {
                selectedTransaction.ChargedAccount = accountDataAccess.SelectedAccount;
            }

            if (accountDataAccess.AllAccounts.Any())
            {
                selectedTransaction.ChargedAccount = accountDataAccess.AllAccounts.First();
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