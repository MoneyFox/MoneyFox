using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;

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
        public AccountRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        ///     Loads the account and all associated Payments from the database.
        /// </summary>
        /// <param name="id">id of the account to load.</param>
        /// <returns>Loaded Account</returns>
        public override async Task<AccountEntity> GetById(int id)
        {
            return await DbSet
                .Include(x => x.ChargedPayments).ThenInclude(p => p.Category)
                .Include(x => x.ChargedPayments).ThenInclude(p => p.TargetAccount)
                .Include(x => x.TargetedPayments).ThenInclude(p => p.Category)
                .Include(x => x.TargetedPayments).ThenInclude(p => p.ChargedAccount)
                .Include(x => x.ChargedRecurringPayments).ThenInclude(p => p.Category)
                .Include(x => x.ChargedRecurringPayments).ThenInclude(p => p.TargetAccount)
                .Include(x => x.TargetedRecurringPayments).ThenInclude(p => p.Category)
                .Include(x => x.TargetedRecurringPayments).ThenInclude(p => p.ChargedAccount)
                .FirstAsync(x => x.Id == id);
        }
    }
}