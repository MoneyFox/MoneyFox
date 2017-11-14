using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Grants access to the data stored in the account table on the database.
    ///     To commit changes use the UnitOfWork.
    /// </summary>
    public class AccountRepository : RepositoryBase<AccountEntity>, IAccountRepository
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public AccountRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }

        /// <inheritdoc />
        public async Task<string> GetName(int id)
        {
            return await DbContext.Set<AccountEntity>().Select(x => x.Name).FirstAsync();
        }

        protected override void AttachForeign(AccountEntity entity)
        {
            if (entity.ChargedPayments != null && entity.ChargedPayments.Any())
            {
                DbContext.Attach(entity.ChargedPayments);
            }
            if (entity.TargetedPayments != null && entity.TargetedPayments.Any())
            {
                DbContext.Attach(entity.TargetedPayments);
            }
            if (entity.ChargedRecurringPayments != null && entity.ChargedRecurringPayments.Any())
            {
                DbContext.Attach(entity.ChargedRecurringPayments);
            }
            if (entity.TargetedRecurringPayments != null && entity.TargetedRecurringPayments.Any())
            {
                DbContext.Attach(entity.TargetedRecurringPayments);
            }
        }

        /// <summary>
        ///     Loads the account and all associated Payments from the database.
        /// </summary>
        /// <param name="id">id of the account to load.</param>
        /// <returns>Loaded Account</returns>
        public override async Task<AccountEntity> GetById(int id)
        {
            return await DbContext.Set<AccountEntity>()
                .Include(x => x.ChargedPayments).ThenInclude(p => p.Category)
                .Include(x => x.ChargedPayments).ThenInclude(p => p.TargetAccount)
                .Include(x => x.TargetedPayments).ThenInclude(p => p.Category)
                .Include(x => x.TargetedPayments).ThenInclude(p => p.ChargedAccount)
                .Include(x => x.ChargedRecurringPayments).ThenInclude(p => p.Category)
                .Include(x => x.ChargedRecurringPayments).ThenInclude(p => p.TargetAccount)
                .Include(x => x.TargetedRecurringPayments).ThenInclude(p => p.Category)
                .Include(x => x.TargetedRecurringPayments).ThenInclude(p => p.ChargedAccount)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}