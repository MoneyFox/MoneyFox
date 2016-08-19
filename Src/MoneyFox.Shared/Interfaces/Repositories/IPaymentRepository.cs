using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>, ISelectedProperty<Payment>
    {
    }
}