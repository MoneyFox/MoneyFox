using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Grants access to the data stored in the contact table on the database.
    ///     To commit changes use the UnitOfWork.
    /// </summary>
    public class CategoryRepository : RepositoryBase<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}