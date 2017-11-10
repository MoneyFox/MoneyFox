using EntityFramework.DbContextScope.Interfaces;
using MoneyFox.DataAccess.Entities;

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
        public RecurringPaymentRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }

        protected override void AttachForeign(RecurringPaymentEntity entity)
        {
            DbContext.Attach(entity.ChargedAccount);
            if (entity.TargetAccount != null)
            {
                DbContext.Attach(entity.ChargedAccount);
            }
            if (entity.Category != null)
            {
                DbContext.Attach(entity.Category);
            }
        }
    }
}