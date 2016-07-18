using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>, ISelectedProperty<Payment>
    {
    }
}