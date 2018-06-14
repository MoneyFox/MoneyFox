using System;
using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;

namespace MoneyFox.DataAccess.QueryExtensions
{
    /// <summary>
    ///     Provides Extensions for categories queries.
    /// </summary>
    public static class CategoryGroupQueryExtensions
    {
        /// <summary>
        ///     Adds a filter for CategoryGroups who do not have a Null name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query with the added filter.</returns>
        public static IQueryable<CategoryGroupEntity> NameNotNull(this IQueryable<CategoryGroupEntity> query)
        {
            return query.Where(group => !string.IsNullOrWhiteSpace(group.Name));
        }

        /// <summary>
        ///     Adds a filter to a query to find all category with the passed name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="name">Name to filter for</param>
        /// <returns>Query with the added filter.</returns>
        public static IQueryable<CategoryGroupEntity> NameEquals(this IQueryable<CategoryGroupEntity> query, string name)
        {
            return query.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Ordery a category query by name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Ordered Query</returns>
        public static IQueryable<CategoryGroupEntity> OrderByName(this IQueryable<CategoryGroupEntity> query)
        {
            return query.OrderBy(group => group.Name);
        }

        /// <summary>
        ///     Selects a <see cref="Category" /> for every item in a query.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Selection query.</returns>
        public static IQueryable<CategoryGroup> SelectGroup(this IQueryable<CategoryGroupEntity> query)
        {
            return query.Select(group => new CategoryGroup(group));
        }
    }
}