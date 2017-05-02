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
        public AccountRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override async Task<AccountEntity> GetById(int id)
        {
            return await DbSet
                .Include(x => x.ChargedPayments)
                .Include(x => x.TargetedPayments)
                .Include(x => x.ChargedRecurringPayments)
                .Include(x => x.TargetedRecurringPayments)
                .FirstAsync(x => x.Id == id);
        }
    }
}