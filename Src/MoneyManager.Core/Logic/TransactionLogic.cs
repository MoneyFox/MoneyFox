using System;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Logic
{
    public class TransactionLogic
    {
        public static async Task SaveTransaction(FinancialTransaction transaction, bool refreshRelatedList = false,
            bool skipRecurring = false)
        {
            if (transaction.IsRecurring && !skipRecurring)
            {
                var recurringTransaction =
                    RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);
                RecurringTransactionRepository.Save(recurringTransaction);
                TransactionRepository.Save(transaction);
                transaction.RecurringTransaction = recurringTransaction;
            }

            TransactionRepository.Save(transaction);

            if (refreshRelatedList)
            {
                Mvx.Resolve<TransactionListViewModel>()
                    .SetRelatedTransactions(AccountRepository.Selected);
            }
            await AccountLogic.AddTransactionAmount(transaction);
        }

        //TODO: Move to VM / Refactor this
        public static void GoToAddTransaction(TransactionType transactionType, bool refreshRelatedList = false)
        {
            Mvx.Resolve<CategoryListViewModel>().IsSettingCall = false;
            AddTransactionView.IsEdit = false;
            AddTransactionView.IsEndless = true;
            AddTransactionView.RefreshRealtedList = refreshRelatedList;
            AddTransactionView.IsTransfer = transactionType == TransactionType.Transfer;
            SetDefaultTransaction(transactionType);
            SetDefaultAccount();
        }

        public static void PrepareEdit(FinancialTransaction transaction)
        {
            Mvx.Resolve<CategoryListViewModel>().IsSettingCall = false;
            AddTransactionView.IsEdit = true;
            AddTransactionView.IsTransfer = transaction.Type == (int) TransactionType.Transfer;
            if (transaction.ReccuringTransactionId.HasValue && transaction.RecurringTransaction != null)
            {
                AddTransactionView.IsEndless = transaction.RecurringTransaction.IsEndless;
                AddTransactionView.Recurrence = transaction.RecurringTransaction.Recurrence;
            }

            //Ultra dirty monkey patch for a problem with displaying the selected account.
            //TODO: Do this in a nicer way!
            transaction.ChargedAccount = AccountRepository.Data.First(x => x.Id == transaction.ChargedAccountId);
            TransactionRepository.Selected = transaction;
        }

        public static async Task DeleteTransaction(FinancialTransaction transaction, bool skipConfirmation = false)
        {
            //TODO: refactor
            if (skipConfirmation) //|| await Utilities.IsDeletionConfirmed())
            {
                await CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionLogic.Delete(transaction.RecurringTransaction));

                TransactionRepository.Delete(transaction);

                await AccountLogic.RemoveTransactionAmount(transaction);
                AccountLogic.RefreshRelatedTransactions();
                Mvx.Resolve<BalanceViewModel>().UpdateBalance();
            }
        }

        public static void DeleteAssociatedTransactionsFromDatabase(Account account)
        {
            if (TransactionRepository.Data == null)
            {
                return;
            }

            var transactionsToDelete = TransactionRepository.GetRelatedTransactions(account);

            foreach (var transaction in transactionsToDelete)
            {
                TransactionRepository.Delete(transaction);
            }
        }

        public static async Task UpdateTransaction(FinancialTransaction transaction)
        {
            CheckIfRecurringWasRemoved(transaction);
            await AccountLogic.AddTransactionAmount(transaction);
            TransactionRepository.Save(transaction);

            var recurringTransaction =
                RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);

            await
                CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionRepository.Save(recurringTransaction));

            AccountLogic.RefreshRelatedTransactions();
        }

        private static async Task CheckForRecurringTransaction(FinancialTransaction transaction,
            Action recurringTransactionAction)
        {
            if (!transaction.IsRecurring)
            {
            }

            //TODO: refactor this to use the dialog service
            //var dialog =
            //    new MessageDialog(Translation.GetTranslation("ChangeSubsequentTransactionsMessage"),
            //        Translation.GetTranslation("ChangeSubsequentTransactionsTitle"));

            //dialog.Commands.Add(new UICommand(Translation.GetTranslation("RecurringLabel")));
            //dialog.Commands.Add(new UICommand(Translation.GetTranslation("JustThisLabel")));

            //dialog.DefaultCommandIndex = 1;

            //var result = await dialog.ShowAsync();

            //if (result.Label == Translation.GetTranslation("RecurringLabel"))
            //{
            //    recurringTransactionAction();
            //}
        }

        private static void CheckIfRecurringWasRemoved(FinancialTransaction transaction)
        {
            if (!transaction.IsRecurring && transaction.ReccuringTransactionId.HasValue)
            {
                RecurringTransactionRepository.Delete(transaction.RecurringTransaction);
                transaction.ReccuringTransactionId = null;
            }
        }

        private static void SetDefaultTransaction(TransactionType transactionType)
        {
            SelectedTransaction = new FinancialTransaction
            {
                Type = (int) transactionType,
                IsExchangeModeActive = false,
                Currency = Mvx.Resolve<SettingDataAccess>().DefaultCurrency
            };
        }

        private static void SetDefaultAccount()
        {
            try
            {
                if (AccountRepository.Data.Any())
                {
                    SelectedTransaction.ChargedAccount = AccountRepository.Data.First();
                }

                if (AccountRepository.Data.Any() && Settings.DefaultAccount != -1)
                {
                    SelectedTransaction.ChargedAccount =
                        AccountRepository.Data.FirstOrDefault(x => x.Id == Settings.DefaultAccount);
                }

                if (AccountRepository.Selected != null)
                {
                    SelectedTransaction.ChargedAccount = AccountRepository.Selected;
                }
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
            }
        }

        public static async Task ClearTransactions()
        {
            var transactions = TransactionRepository.GetUnclearedTransactions();
            foreach (var transaction in transactions)
            {
                try
                {
                    await AccountLogic.AddTransactionAmount(transaction);
                }
                catch (Exception ex)
                {
                    InsightHelper.Report(ex);
                }
            }
        }

        #region Properties

        private static IRepository<Account> AccountRepository
            => Mvx.Resolve<IRepository<Account>>();

        private static ITransactionRepository TransactionRepository
            => Mvx.Resolve<ITransactionRepository>();

        private static FinancialTransaction SelectedTransaction
        {
            get { return Mvx.Resolve<ITransactionRepository>().Selected; }
            set { Mvx.Resolve<ITransactionRepository>().Selected = value; }
        }

        private static IRepository<RecurringTransaction> RecurringTransactionRepository
            => Mvx.Resolve<IRepository<RecurringTransaction>>();

        private static AddTransactionViewModel AddTransactionView
            => Mvx.Resolve<AddTransactionViewModel>();

        private static SettingDataAccess Settings => Mvx.Resolve<SettingDataAccess>();

        #endregion Properties
    }
}