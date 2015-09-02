using System;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Core.Logic;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Manager
{
    public class TransactionManager
    {
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<RecurringTransaction> recurringTransactionRepository;
        private readonly ITransactionRepository transactionRepository;

        /// <summary>
        ///     Creates an TransactionManager object.
        /// </summary>
        /// <param name="transactionRepository">Instance of <see cref="ITransactionRepository" /></param>
        /// <param name="accountRepository">Instance of <see cref="IRepository{T}" /></param>
        public TransactionManager(ITransactionRepository transactionRepository,
            IRepository<Account> accountRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
        }

        public void SaveTransaction(FinancialTransaction transaction, bool refreshRelatedList = false,
            bool skipRecurring = false)
        {
            if (transaction.IsRecurring && !skipRecurring)
            {
                //TODO: refactor
                //var recurringTransaction = RecurringTransactionHelper.GetRecurringFromFinancialTransaction(transaction);
                //recurringTransactionRepository.Save(recurringTransaction);
                //transaction.RecurringTransaction = recurringTransaction;
            }

            transactionRepository.Save(transaction);

            AddTransactionAmount(transaction);
        }

        public async Task DeleteTransaction(FinancialTransaction transaction, bool skipConfirmation = false)
        {
            //TODO: refactor
            if (skipConfirmation) //|| await Utilities.IsDeletionConfirmed())
            {
                await CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionLogic.Delete(transaction.RecurringTransaction));

                transactionRepository.Delete(transaction);
                RemoveTransactionAmount(transaction);
            }
        }

        public void DeleteAssociatedTransactionsFromDatabase(Account account)
        {
            if (transactionRepository.Data == null)
            {
                return;
            }

            var transactionsToDelete = transactionRepository.GetRelatedTransactions(account);

            foreach (var transaction in transactionsToDelete)
            {
                transactionRepository.Delete(transaction);
            }
        }

        public async Task UpdateTransaction(FinancialTransaction transaction)
        {
            CheckIfRecurringWasRemoved(transaction);
            AddTransactionAmount(transaction);
            transactionRepository.Save(transaction);

            //TODO: refactor
            //var recurringTransaction = RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);

            //await
            //    CheckForRecurringTransaction(transaction,
            //        () => recurringTransactionRepository.Save(recurringTransaction));
        }

        private void CheckIfRecurringWasRemoved(FinancialTransaction transaction)
        {
            if (!transaction.IsRecurring && transaction.ReccuringTransactionId.HasValue)
            {
                recurringTransactionRepository.Delete(transaction.RecurringTransaction);
                transaction.ReccuringTransactionId = null;
            }
        }

        private async Task CheckForRecurringTransaction(FinancialTransaction transaction,
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

        public void ClearTransactions()
        {
            var transactions = transactionRepository.GetUnclearedTransactions();
            foreach (var transaction in transactions)
            {
                try
                {
                    AddTransactionAmount(transaction);
                }
                catch (Exception ex)
                {
                    InsightHelper.Report(ex);
                }
            }
        }

        /// <summary>
        ///     Removes the transaction Amount from the selected account
        /// </summary>
        /// <param name="transaction">Transaction to remove the account from.</param>
        public void RemoveTransactionAmount(FinancialTransaction transaction)
        {
            if (transaction.Cleared)
            {
                PrehandleRemoveIfTransfer(transaction);

                Func<double, double> amountFunc = x =>
                    transaction.Type == (int)TransactionType.Income
                        ? -x
                        : x;

                HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
            }
        }

        /// <summary>
        ///     Adds the transaction Amount from the selected account
        /// </summary>
        /// <param name="transaction">Transaction to add the account from.</param>
        public void AddTransactionAmount(FinancialTransaction transaction)
        {
            PrehandleAddIfTransfer(transaction);

            Func<double, double> amountFunc = x =>
                transaction.Type == (int)TransactionType.Income
                    ? x
                    : -x;

            HandleTransactionAmount(transaction, amountFunc, GetChargedAccountFunc());
        }

        private void PrehandleRemoveIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int)TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => -x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private void HandleTransactionAmount(FinancialTransaction transaction,
            Func<double, double> amountFunc,
            Func<FinancialTransaction, Account> getAccountFunc)
        {
            if (transaction.ClearTransactionNow)
            {
                var account = getAccountFunc(transaction);
                if (account == null)
                {
                    return;
                }

                account.CurrentBalance += amountFunc(transaction.Amount);
                transaction.Cleared = true;

                accountRepository.Save(account);
            } else
            {
                transaction.Cleared = false;
            }
            transactionRepository.Save(transaction);

        }

        private void PrehandleAddIfTransfer(FinancialTransaction transaction)
        {
            if (transaction.Type == (int)TransactionType.Transfer)
            {
                Func<double, double> amountFunc = x => x;
                HandleTransactionAmount(transaction, amountFunc, GetTargetAccountFunc());
            }
        }

        private Func<FinancialTransaction, Account> GetTargetAccountFunc()
        {
            Func<FinancialTransaction, Account> targetAccountFunc =
                trans => accountRepository.Data.FirstOrDefault(x => x.Id == trans.TargetAccount.Id);
            return targetAccountFunc;
        }

        private Func<FinancialTransaction, Account> GetChargedAccountFunc()
        {
            Func<FinancialTransaction, Account> accountFunc =
                trans => accountRepository.Data.FirstOrDefault(x => x.Id == trans.ChargedAccount.Id);
            return accountFunc;
        }
    }
}