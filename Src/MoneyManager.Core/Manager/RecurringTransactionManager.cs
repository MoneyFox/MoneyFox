using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Manager
{
    public class RecurringTransactionManager : IRecurringTransactionManager
    {
        private readonly ITransactionRepository transactionRepository;

        public RecurringTransactionManager(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        /// <summary>
        ///     Checks if one of the recurring transaction has to be repeated
        /// </summary>
        public async Task CheckRecurringTransactions()
        {
            var task = Task.Run(() => CheckRecurring());
            await task;
        }

        private void CheckRecurring()
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
            var transcationList = transactionRepository.LoadRecurringList(
                x => x.ReccuringTransactionId == transaction.ReccuringTransactionId)
                .OrderBy(x => x.Date);

            return transcationList.LastOrDefault();
        }
    }
}