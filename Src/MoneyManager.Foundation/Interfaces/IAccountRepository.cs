using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        void AddTransactionAmount(Payment transaction);
        void RemoveTransactionAmount(Payment transaction);
    }
}