using System.Threading.Tasks;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces
{
    public interface ITransactionManager
    {
        void DeleteAssociatedTransactionsFromDatabase(Account account);

        Task<bool> CheckForRecurringTransaction(Payment transaction);

        void ClearTransactions();

        void RemoveRecurringForTransactions(RecurringPayment recTrans);
    }
}