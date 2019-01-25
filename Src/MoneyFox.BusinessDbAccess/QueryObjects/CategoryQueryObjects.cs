using System;
using System.Linq;
using MoneyFox.DataLayer.Entities;

namespace MoneyFox.DataAccess.QueryExtensions
{
    /// <summary>
    ///     Provides Extensions for categories queries.
    /// </summary>
    public static class CategoryQueryObjects
    {
        /// <summary>
        ///     Adds a filter for Categories who do not have a Null name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query with the added filter.</returns>
        public static IQueryable<Category> NameNotNull(this IQueryable<Category> query)
        {
            return query.Where(category => !string.IsNullOrWhiteSpace(category.Name));
        }

        /// <summary>
        ///     Adds a filter to a query to find all category with the passed name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="name">Name to filter for</param>
        /// <returns>Query with the added filter.</returns>
        public static IQueryable<Category> NameEquals(this IQueryable<Category> query, string name)
        {
            return query.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///      Adds a filter for Categories who have a certain string in the name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="searchterm">Searchterm to look for.</param>
        /// <returns>Query with the added filter.</returns>
        public static IQueryable<Category> NameContains(this IQueryable<Category> query, string searchterm)
        {
            return query.Where(category => category.Name.ToLower().Contains(searchterm.ToLower()));
        }

        /// <summary>
        ///      Ordery a category query by name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Ordered Query</returns>
        public static IQueryable<Category> OrderByName(this IQueryable<Category> query)
        {
            return query.OrderBy(category => category.Name);
        }
    }
}
