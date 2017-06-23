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
        /// <summary>
        ///     Default Constructor
        /// </summary>
        /// <param name="dbFactory">DbFactory to get Context.</param>
        public UnitOfWork(IDbFactory dbFactory)
        {
            DbContext = dbFactory.Init().Result;
        }

        private ApplicationContext DbContext { get; }

        /// <inheritdoc />
        public async Task Commit()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}