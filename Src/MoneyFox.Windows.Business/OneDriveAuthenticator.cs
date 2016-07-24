using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Windows.Business
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private IOneDriveClient oneDriveClient;

        public async Task<IOneDriveClient> LoginAsync()
        {
            try
            {
                if (oneDriveClient == null)
                {
                    oneDriveClient = OneDriveClientExtensions.GetUniversalClient(ServiceConstants.Scopes);
                    await oneDriveClient.AuthenticateAsync();
                }

                if (!oneDriveClient.IsAuthenticated)
                {
                    await oneDriveClient.AuthenticateAsync();
                }

                return oneDriveClient;
            }
            catch (OneDriveException exception)
            {
                // Swallow authentication cancelled exceptions
                if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString()))
                {
                    if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString()))
                    {
                        throw new BackupException("Authentication Failed");
                    }
                }
            }
            return oneDriveClient;
        }

        public async Task LogoutAsync() {
            await oneDriveClient.SignOutAsync();
        }
    }
}