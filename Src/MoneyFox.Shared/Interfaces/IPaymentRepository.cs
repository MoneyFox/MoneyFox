using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;

namespace MoneyFox.Shared.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>, ISelectedProperty<Payment>
    {
    }
}