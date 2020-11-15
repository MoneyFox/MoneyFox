using Microsoft.Graph;

namespace MoneyFox.Application.Common.CloudBackup
{
    /// <summary>
    /// Holds info about logged user.
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// User's name from Microsoft account.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Users email from Microsoft account.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Users profile photo from Microsoft account.
        /// </summary>
        public ProfilePhoto? Photo { get; set; }

        /// <summary>
        /// Set's informations 
        /// </summary>
        /// <param name="user"></param>
        public void SetUserAccount(User user)
        {
            Name = user.DisplayName;
            Email = string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail;
            Photo = user.Photo;
        }

        /// <summary>
        /// Get's UserAccount class.
        /// </summary>
        /// <returns>Returns UserAccount which holds informations about logged user.</returns>
        public UserAccount GetUserAccount() => this;
    }
}
