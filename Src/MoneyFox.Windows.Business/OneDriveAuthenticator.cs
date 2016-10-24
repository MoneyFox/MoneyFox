using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Windows.Business
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        /// <summary>
        ///     Perform an async Login Request
        /// </summary>
        public async Task<IOneDriveClient> LoginAsync()
        {
            try
            {
                var msaAuthenticationProvider = new MsaAuthenticationProvider(
                    ServiceConstants.MSA_CLIENT_ID,
                    ServiceConstants.RETURN_URL,
                    ServiceConstants.Scopes,
                    new CredentialVault(ServiceConstants.MSA_CLIENT_ID));

                await msaAuthenticationProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                return new OneDriveClient(ServiceConstants.BASE_URL, msaAuthenticationProvider);
            }
            catch (ServiceException serviceException)
            {
                Debug.WriteLine(serviceException);
                throw new BackupException("Authentication Failed with Graph.ServiceException", serviceException);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new BackupException("Authentication Failed", ex);
            }
        }

        /// <summary>
        ///     Perform an async Logout Request
        /// </summary>
        public async Task LogoutAsync()
        {
            await new MsaAuthenticationProvider(
                    ServiceConstants.MSA_CLIENT_ID,
                    ServiceConstants.RETURN_URL,
                    ServiceConstants.Scopes,
                    new CredentialVault(ServiceConstants.MSA_CLIENT_ID))
                .SignOutAsync();
        }
    }
}