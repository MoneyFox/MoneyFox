using MoneyFox.Core.Model;

namespace MoneyFox.Core.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        void AddPaymentAmount(PaymentViewModel payment);

        void RemovePaymentAmount(PaymentViewModel payment);

        void RemovePaymentAmount(PaymentViewModel payment, Account account);
    }
}