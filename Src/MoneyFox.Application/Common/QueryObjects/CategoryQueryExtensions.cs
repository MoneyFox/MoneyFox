using MoneyFox.Domain.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MoneyFox.Application.Common.QueryObjects
{
    /// <summary>
    /// Provides Extensions for categories queries.
    /// </summary>
    public static class CategoryQueryExtensions
    {
        /// <summary>
        /// Adds a filter to a query to find all category with the passed name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="name">Name to filter for</param>
        /// <returns>Query with the added filter.</returns>
        [SuppressMessage("Minor Code Smell", "S4058:Overloads with a \"StringComparison\" parameter should be used", Justification = "Since used on database can't set locale.")]
        public static IQueryable<Category> NameEquals(this IQueryable<Category> query, string name)
            => query.Where(x => x.Name.ToUpper().Equals(name.ToUpper()));


        /// <summary>
        /// Adds a filter for Categories who have a certain string in the name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="searchterm">Search term to look for.</param>
        /// <returns>Query with the added filter.</returns>
        [SuppressMessage("Minor Code Smell", "S4058:Overloads with a \"StringComparison\" parameter should be used", Justification = "Since used on database can't set locale.")]
        [SuppressMessage("Minor Code Smell", "S1449:Culture should be specified for \"string\" operations", Justification = "Since used on database can't set locale.")]
        public static IEnumerable<Category> WhereNameContains(this IEnumerable<Category> query, string searchterm)
            => query.Where(category => category.Name.ToUpper().IndexOf(searchterm.ToUpper()) >= 0);

        /// <summary>
        /// Orders a category query by name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Ordered Query</returns>
        public static IQueryable<Category> OrderByName(this IQueryable<Category> query)
            => query.OrderBy(category => category.Name);
    }
}
