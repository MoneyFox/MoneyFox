#region

using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;

#endregion

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

        private static SettingDataAccess settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        #endregion Properties

        public static async Task SaveTransaction(FinancialTransaction transaction, bool refreshRelatedList = false,
            bool skipRecurring = false)
        {
            if (transaction.IsRecurring && !skipRecurring)
            {
                var recurringTransaction =
                    RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);
                recurringTransactionData.Save(transaction, recurringTransaction);
                transaction.RecurringTransaction = recurringTransaction;
            }

            transactionData.Save(transaction);

            if (refreshRelatedList)
            {
                ServiceLocator.Current.GetInstance<TransactionListViewModel>()
                    .SetRelatedTransactions(accountDataAccess.SelectedAccount.Id);
            }
            await AccountLogic.AddTransactionAmount(transaction);
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

        public static async void DeleteTransaction(FinancialTransaction transaction)
        {
            if (await Utilities.IsDeletionConfirmed())
            {
                await CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionLogic.Delete(transaction.RecurringTransaction));

                transactionData.Delete(transaction);

                await AccountLogic.RemoveTransactionAmount(transaction);
                AccountLogic.RefreshRelatedTransactions();
                ServiceLocator.Current.GetInstance<BalanceViewModel>().UpdateBalance();
            }
        }

        public static void DeleteAssociatedTransactionsFromDatabase(int accountId)
        {
            if (transactionData.AllTransactions == null) return;

            foreach (var transaction in
                transactionData.AllTransactions.Where(x => x.ChargedAccountId == accountId))
            {
                transactionData.Delete(transaction);
            }
        }

        public static async void UpdateTransaction(FinancialTransaction transaction)
        {
            CheckIfRecurringWasRemoved(transaction);
            await AccountLogic.AddTransactionAmount(transaction);
            transactionData.Update(transaction);

            var recurringTransaction =
                RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);

            await
                CheckForRecurringTransaction(transaction,
                    () => recurringTransactionData.Update(transaction, recurringTransaction));

            AccountLogic.RefreshRelatedTransactions();
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
                Type = (int) transactionType,
                IsExchangeModeActive = false,
                Currency = ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency
            };
        }

        private static void SetDefaultAccount()
        {
            if (accountDataAccess.AllAccounts.Any() && settings.DefaultAccount == -1)
            {
                selectedTransaction.ChargedAccount = accountDataAccess.AllAccounts.First(x => x.Id == settings.DefaultAccount);
            }
            else if (accountDataAccess.SelectedAccount != null)
            {
                selectedTransaction.ChargedAccount = accountDataAccess.SelectedAccount;
            } 
            else if (accountDataAccess.AllAccounts.Any())
            {
                selectedTransaction.ChargedAccount = accountDataAccess.AllAccounts.First();
            }
        }

        public async static Task ClearTransactions()
        {
            var transactions = transactionData.GetUnclearedTransactions();
            foreach (var transaction in transactions)
            {
                await AccountLogic.AddTransactionAmount(transaction);
            }
        }
    }
}