using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Grants access to the data stored in the contact table on the database.
    ///     To commit changes use the UnitOfWork.
    /// </summary>
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}