using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;

namespace MoneyFox.Core.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        void AddPaymentAmount(Payment payment);

        void RemovePaymentAmount(Payment payment);

        void RemovePaymentAmount(Payment payment, Account account);
    }
}