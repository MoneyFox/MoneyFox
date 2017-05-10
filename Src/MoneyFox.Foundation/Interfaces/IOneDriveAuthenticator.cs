using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Foundation.Exceptions;

namespace MoneyFox.Foundation.Interfaces
{
    /// <summary>
    ///     Provides authentication to the OneDrive Service
    /// </summary>
    public interface IOneDriveAuthenticator
    {
        /// <summary>
        ///     Perform an async Login Request
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<IOneDriveClient> LoginAsync();

        /// <summary>
        ///     Perform an async Logout Request
        /// </summary>
        Task LogoutAsync();
    }
}