namespace MoneyFox.Core.Common.Extensions.QueryObjects;

using System.Linq;
using System.Threading.Tasks;
using Domain.Aggregates.AccountAggregate;
using Microsoft.EntityFrameworkCore;

/// <summary>
///     Query Objects for account queries.
/// </summary>
public static class AccountQueriesExtensions
{
    /// <summary>
    ///     Adds a filter for the accountId
    /// </summary>
    public static IQueryable<Account> WithId(this IQueryable<Account> query, int id)
    {
        return query.Where(x => x.Id == id);
    }

    /// <summary>
    ///     Adds a filter to a query for excluded accounts
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query with a filter for excluded accounts.</returns>
    public static IQueryable<Account> AreNotExcluded(this IQueryable<Account> query)
    {
        return query.Where(x => !x.IsExcluded);
    }

    /// <summary>
    ///     Adds a filter to a query for not excluded accounts
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query with a filter for not excluded accounts.</returns>
    public static IQueryable<Account> AreExcluded(this IQueryable<Account> query)
    {
        return query.Where(x => x.IsExcluded);
    }

    /// <summary>
    ///     Adds a filter for active accounts
    /// </summary>
    public static IQueryable<Account> AreActive(this IQueryable<Account> query)
    {
        return query.Where(x => !x.IsDeactivated);
    }

    /// <summary>
    ///     Order a query by a name.
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query ordered by name.</returns>
    public static IQueryable<Account> OrderByName(this IQueryable<Account> query)
    {
        return query.OrderBy(x => x.Name);
    }

    /// <summary>
    ///     Order a query if included or excluded.
    /// </summary>
    /// <param name="query">Existing query.</param>
    /// <returns>Query ordered by inclusion.</returns>
    public static IQueryable<Account> OrderByInclusion(this IQueryable<Account> query)
    {
        return query.OrderBy(x => x.IsExcluded);
    }

    /// <summary>
    ///     Checks if there is an active account with the passed name.
    /// </summary>
    public static async Task<bool> AnyWithNameAsync(this IQueryable<Account> query, string name, int id = 0)
    {
        return await query.AnyAsync(x => x.Name.ToUpper() == name.ToUpper() && !x.IsDeactivated && x.Id != id);
    }
}
