using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Concret repository for accounts.
    /// </summary>
    public interface IAccountRepository : IRepository<AccountEntity>
    {
        /// <summary>
        ///     Returns the name of the account with the passed id.
        /// </summary>
        /// <param name="id">Id of the account to load.</param>
        /// <returns>Accountname.</returns>
        string GetName(int id);
    }
}