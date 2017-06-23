using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Grants access to the data stored in the payment table on the database.
    ///     To commit changes use the UnitOfWork.
    /// </summary>
    public class PaymentRepository : RepositoryBase<PaymentEntity>, IPaymentRepository
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public PaymentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        ///     Loads the first payment who matches the id including the associated recurring payment, accounts and categories.
        /// </summary>
        /// <param name="id">Payment ID to lookup</param>
        /// <returns>Loaded Payment.</returns>
        public override Task<PaymentEntity> GetById(int id)
        {
            return DbSet
                .Include(x => x.RecurringPayment)
                .Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .FirstAsync(x => x.Id == id);
        }
    }
}