using MoneyFox.DataAccess.Entities;

namespace MoneyFox.Service.Pocos
{
    /// <summary>
    ///     Business object for account data.
    /// </summary>
    public class Account
    {
        /// <summary>
        ///     Default constructor. Will Create a new AccountEntity
        /// </summary>
        public Account()
        {
            Data = new AccountEntity();
        }

        /// <summary>
        ///     Set the data for this object.
        /// </summary>
        /// <param name="account">Account data to wrap.</param>
        public Account(AccountEntity account)
        {
            Data = account;
        }

        /// <summary>
        ///     Accountdata
        /// </summary>
        public AccountEntity Data { get; set; }
    }
}
