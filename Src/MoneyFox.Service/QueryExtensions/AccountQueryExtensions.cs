using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Service.QueryExtensions
{
    /// <summary>
    ///     Provides Extensions for account queries.
    /// </summary>
    public static class AccountQueryExtensions
    {
        /// <summary>
        ///     Adds a filter to a query for excluded accounts
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query with a filter for excluded accounts.</returns>
        public static IQueryable<AccountEntity> AreNotExcluded(this IQueryable<AccountEntity> query)
        {
            return query.Where(x => !x.IsExcluded);
        }

        /// <summary>
        ///     Adds a filter to a query for not excluded accounts
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query with a filter for not excluded accounts.</returns>
        public static IQueryable<AccountEntity> AreExcluded(this IQueryable<AccountEntity> query)
        {
            return query.Where(x => x.IsExcluded);
        }

        /// <summary>
        ///     Selects a <see cref="Account"/> for every item in a query.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Selection query.</returns>
        public static IQueryable<Account> SelectAccounts(this IQueryable<AccountEntity> query)
        {
            return query.Select(account => new Account(account));
        }
    }
}
