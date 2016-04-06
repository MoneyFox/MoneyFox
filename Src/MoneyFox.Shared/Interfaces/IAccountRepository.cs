using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        void AddPaymentAmount(Payment payment);

        void RemovePaymentAmount(Payment payment);

        void RemovePaymentAmount(Payment payment, Account account);
    }
}