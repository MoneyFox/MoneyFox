using System.Threading.Tasks;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IRecurringTransactionManager
    {
        Task CheckRecurringTransactions();
    }
}