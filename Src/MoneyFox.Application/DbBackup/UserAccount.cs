namespace MoneyFox.Application.DbBackup
{
    /// <summary>
    ///     Holds info about logged user.
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        ///     User's name from Microsoft account.
        /// </summary>
        public string? Name { get; private set; }

        /// <summary>
        ///     Users email from Microsoft account.
        /// </summary>
        public string? Email { get; private set; }

        /// <summary>
        ///     Set's informations
        /// </summary>
        /// <param name="user"></param>
        public void SetUserAccount(string displayName, string email)
        {
            Name = displayName;
            Email = email;
        }

        /// <summary>
        ///     Get's UserAccount class.
        /// </summary>
        /// <returns>Returns UserAccount which holds informations about logged user.</returns>
        public UserAccount GetUserAccount() => this;
    }
}