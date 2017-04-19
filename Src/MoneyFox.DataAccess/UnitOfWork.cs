using System.Threading.Tasks;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Enables to save changes made over several repositories in one transaction.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private ApplicationContext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public ApplicationContext DbContext => dbContext ?? (dbContext = dbFactory.Init());

        /// <summary>
        ///     Saves all pending changes to the database.
        /// </summary>
        public async Task Commit()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}