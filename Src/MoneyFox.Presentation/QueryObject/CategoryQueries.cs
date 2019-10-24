using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.QueryObject
{
    /// <summary>
    ///     Query Objects for account queries.
    /// </summary>
    public static class CategoryQueries
    {
        /// <summary>
        ///     Checks if there is an category with the passed name.
        /// </summary>
        public static async Task<bool> AnyWithNameAsync(this IQueryable<CategoryViewModel> query, string name)
        {
            return await query.AnyAsync(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                ;
        }

        /// <summary>
        ///     Checks if there is an account with the passed name.
        /// </summary>
        public static IQueryable<CategoryViewModel> WhereNameEquals(this IQueryable<CategoryViewModel> query, string name)
        {
            return query.Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        ///     Checks if there is an account with the passed name.
        /// </summary>
        public static IQueryable<CategoryViewModel> WhereNameContains(this IQueryable<CategoryViewModel> query, string searchTerm)
        {
            return query.Where(x => x.Name.IndexOf(searchTerm, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }
    }
}
