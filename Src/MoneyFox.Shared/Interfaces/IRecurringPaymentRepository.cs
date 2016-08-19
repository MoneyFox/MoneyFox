using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;

namespace MoneyFox.Shared.Interfaces
{
    public interface IRecurringPaymentRepository : IRepository<RecurringPayment>, IData<RecurringPayment>
    {
        
    }
}