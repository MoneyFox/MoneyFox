using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        void AddTransactionAmount(FinancialTransaction transaction);
        void RemoveTransactionAmount(FinancialTransaction transaction);
    }
}
