using System.Threading.Tasks;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Enables to save changes made over several repositories in one transaction.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Saves all pending changes to the database.
        /// </summary>
        Task Commit();
    }

    /// <inheritdoc />
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private ApplicationContext dbContext;

        /// <summary>
        ///     Default Constructor
        /// </summary>
        /// <param name="dbFactory">DbFactory to get Context.</param>
        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        private ApplicationContext DbContext => dbContext ?? (dbContext = dbFactory.Init().Result);

        /// <inheritdoc />
        public async Task Commit()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}