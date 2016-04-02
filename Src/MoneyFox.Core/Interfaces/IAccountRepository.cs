using MoneyFox.Core.DatabaseModels;
using MoneyFox.Core.ViewModels.Models;

namespace MoneyFox.Core.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        void AddPaymentAmount(PaymentViewModel payment);

        void RemovePaymentAmount(PaymentViewModel payment);

        void RemovePaymentAmount(PaymentViewModel payment, Account account);
    }
}