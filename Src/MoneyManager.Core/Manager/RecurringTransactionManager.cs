using System.Linq;
using MoneyManager.Core.Helper;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Manager
{
    public class RecurringTransactionManager
    {
        private readonly ITransactionRepository transactionRepository;

        public RecurringTransactionManager(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        /// <summary>
        ///     Checks if one of the recurring transaction has to be repeated
        /// </summary>
        public void CheckRecurringTransactions()
        {
            var transactionList = transactionRepository.LoadRecurringList();

            foreach (var transaction in transactionList.Where(x => x.ChargedAccount != null))
            {
                var relTransaction = GetLastOccurence(transaction);

                if (RecurringTransactionHelper.CheckIfRepeatable(transaction.RecurringTransaction, relTransaction))
                {
                    transactionRepository.Save(
                        RecurringTransactionHelper.GetFinancialTransactionFromRecurring(transaction.RecurringTransaction));
                }
            }
        }

        private FinancialTransaction GetLastOccurence(FinancialTransaction transaction)
        {
            var transcationList = transactionRepository.Data
                .Where(x => x.ReccuringTransactionId == transaction.ReccuringTransactionId)
                .OrderBy(x => x.Date)
                .ToList();

            return transcationList.Any() ? transcationList.Last() : new FinancialTransaction();
        }
    }
}