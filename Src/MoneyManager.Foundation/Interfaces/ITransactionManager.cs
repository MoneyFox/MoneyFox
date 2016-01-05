using System.Threading.Tasks;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces
{
    public interface ITransactionManager
    {
        void DeleteAssociatedTransactionsFromDatabase(Account account);

        Task<bool> CheckForRecurringTransaction(FinancialTransaction transaction);

        void ClearTransactions();

        void RemoveRecurringForTransactions(RecurringTransaction recTrans);
    }
}