using System;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;

namespace MoneyManager.Core.Manager
{
    public class TransactionManager : ITransactionManager
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly ITransactionRepository transactionRepository;

        /// <summary>
        ///     Creates an TransactionManager object.
        /// </summary>
        /// <param name="transactionRepository">Instance of <see cref="ITransactionRepository" /></param>
        /// <param name="accountRepository">Instance of <see cref="IRepository{T}" /></param>
        /// <param name="dialogService">Instance of <see cref="IDialogService" /></param>
        public TransactionManager(ITransactionRepository transactionRepository,
            IAccountRepository accountRepository, IDialogService dialogService)
        {
            this.accountRepository = accountRepository;
            this.dialogService = dialogService;
            this.transactionRepository = transactionRepository;
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

        public async Task<bool> CheckForRecurringTransaction(FinancialTransaction transaction)
        {
            if (!transaction.IsRecurring)
            {
                return false;
            }

            return
                await
                    dialogService.ShowConfirmMessage(Strings.ChangeSubsequentTransactionsTitle,
                        Strings.ChangeSubsequentTransactionsMessage,
                        Strings.RecurringLabel, Strings.JustThisLabel);
        }

        public void ClearTransactions()
        {
            var transactions = transactionRepository.GetUnclearedTransactions();
            foreach (var transaction in transactions)
            {
                try
                {
                    transaction.IsCleared = true;
                    transactionRepository.Save(transaction);

                    accountRepository.AddTransactionAmount(transaction);
                }
                catch (Exception ex)
                {
                    InsightHelper.Report(ex);
                }
            }
        }

        public void RemoveRecurringForTransactions(RecurringTransaction recTrans)
        {
            try
            {
                var relatedTrans = transactionRepository
                    .Data
                    .Where(x => x.IsRecurring && x.ReccuringTransactionId == recTrans.Id);

                foreach (var transaction in relatedTrans)
                {
                    transaction.IsRecurring = false;
                    transaction.ReccuringTransactionId = 0;
                    transactionRepository.Save(transaction);
                }
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
            }
        }
    }
}