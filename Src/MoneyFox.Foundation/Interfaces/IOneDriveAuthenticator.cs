using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;

namespace MoneyFox.Foundation.Interfaces
{
    public interface IOneDriveAuthenticator
    {
        /// <summary>
        ///     Perform an async Login Request
        /// </summary>
        Task<IOneDriveClient> LoginAsync();

        /// <summary>
        ///     Perform an async Logout Request
        /// </summary>
        Task LogoutAsync();
    }
}