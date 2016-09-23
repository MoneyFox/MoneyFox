using System;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Windows.Business
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private IOneDriveClient oneDriveClient;

        public async Task<IOneDriveClient>LoginAsync()
        {
            try
            {
                if (oneDriveClient != null) return oneDriveClient;


                var msaAuthenticationProvider = new MsaAuthenticationProvider(
                    ServiceConstants.MSA_CLIENT_ID,
                    ServiceConstants.RETURN_URL,
                    ServiceConstants.Scopes,
                    new CredentialVault(ServiceConstants.MSA_CLIENT_ID));

                await msaAuthenticationProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                oneDriveClient = new OneDriveClient(ServiceConstants.BASE_URL, msaAuthenticationProvider);

                return oneDriveClient;
            }
            catch (Exception ex)
            {
                // Swallow authentication cancelled exceptions
                //if (!ex.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString()))
                //{
                //    if (ex.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString()))
                //    {
                //        throw new BackupException("Authentication Failed");
                //    }
                //}

                throw new BackupException("Authentication Failed");
            }
        }
    }
}