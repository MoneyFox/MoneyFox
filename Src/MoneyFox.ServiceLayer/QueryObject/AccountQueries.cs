using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.ServiceLayer.QueryObject
{
    /// <summary>
    ///     Query Objects for account queries.
    /// </summary>
    public static class AccountQueries
    {
        /// <summary>
        ///     Checks if there is an account with the passed name.
        /// </summary>
        public static async Task<bool> AnyWithName(this IQueryable<AccountViewModel> query, string name)
        {
            return await query.AnyAsync(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
