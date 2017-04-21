using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Service.QueryExtensions
{
    /// <summary>
    ///     Provides Extensions for categories queries.
    /// </summary>
    public static class CategoryQueryExtensions
    {        
        /// <summary>
        ///     Selects a <see cref="Category"/> for every item in a query.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Selection query.</returns>
        public static IQueryable<Category> SelectCategories(this IQueryable<CategoryEntity> query)
        {
            return query.Select(category => new Category(category));
        }
    }
}
