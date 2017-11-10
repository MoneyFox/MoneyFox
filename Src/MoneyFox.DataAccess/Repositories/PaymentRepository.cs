using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;

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
        public PaymentRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }

        /// <summary>
        ///     Loads the first payment who matches the id including the associated recurring payment, accounts and categories.
        /// </summary>
        /// <param name="id">Payment ID to lookup</param>
        /// <returns>Loaded Payment.</returns>
        public override async Task<PaymentEntity> GetById(int id)
        {
            return await DbContext.Set<PaymentEntity>()
                .Include(x => x.RecurringPayment)
                .Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .FirstAsync(x => x.Id == id);
        }

        public override void Update(PaymentEntity entity)
        {
            DbContext.Set<PaymentEntity>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;

            DbContext.Set<RecurringPaymentEntity>().Attach(entity.RecurringPayment);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        protected override void AttachForeign(PaymentEntity entity)
        {
            DbContext.Attach(entity.ChargedAccount);
            if (entity.TargetAccount != null)
            {
                DbContext.Attach(entity.TargetAccount);
            }

            if (entity.Category != null)
            {
                DbContext.Attach(entity.Category);
            }
        }
    }
}