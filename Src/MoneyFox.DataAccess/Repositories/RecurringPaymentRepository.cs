using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Grants access to the data stored in the contact table on the database.
    ///     To commit changes use the UnitOfWork.
    /// </summary>
    public class RecurringPaymentRepository : RepositoryBase<RecurringPayment>, IRecurringPaymentRepository
    {
        public RecurringPaymentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}