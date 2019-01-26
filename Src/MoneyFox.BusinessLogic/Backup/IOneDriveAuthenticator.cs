using System.Threading.Tasks;
using Microsoft.Graph;
using MoneyFox.Foundation.Exceptions;

namespace MoneyFox.BusinessLogic.Backup
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
        Task<IGraphServiceClient> LoginAsync();

        /// <summary>
        ///     Perform an async Logout Request
        /// </summary>
        Task LogoutAsync();
    }
}