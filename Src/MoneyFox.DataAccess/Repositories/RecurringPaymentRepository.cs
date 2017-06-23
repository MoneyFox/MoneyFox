using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Grants access to the data stored in the recurring payment table on the database.
    ///     To commit changes use the UnitOfWork.
    /// </summary>
    public class RecurringPaymentRepository : RepositoryBase<RecurringPaymentEntity>, IRecurringPaymentRepository
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public RecurringPaymentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}