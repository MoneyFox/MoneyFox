using MoneyFox.Foundation.DataModels;
using MoneyFox.Shared.Interfaces.Repositories;

namespace MoneyFox.Foundation.Interfaces.Repositories
{
    public interface IPaymentRepository : IRepository<PaymentViewModel>
    {
    }
}