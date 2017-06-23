using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Grants access to the data stored in the category table on the database.
    ///     To commit changes use the UnitOfWork.
    /// </summary>
    public class CategoryRepository : RepositoryBase<CategoryEntity>, ICategoryRepository
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public CategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}