using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        bool AddPaymentAmount(Payment payment);

        bool RemovePaymentAmount(Payment payment);

        bool RemovePaymentAmount(Payment payment, Account account);
    }
}