using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Droid.Business.OneDriveAuth
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private IOneDriveClient oneDriveClient;

        public async Task<IOneDriveClient> LoginAsync()
        {
            if (oneDriveClient == null)
            {
                oneDriveClient = OneDriveClient.GetMicrosoftAccountClient(
                    ServiceConstants.MSA_CLIENT_ID,
                    ServiceConstants.RETURN_URL,
                    ServiceConstants.Scopes,
                    ServiceConstants.MSA_CLIENT_SECRET,
                    null, null,
                    new DroidServiceInfoProvider());
                try 
                {
                    await oneDriveClient.AuthenticateAsync();
                }
                catch (Exception ex)
                {
                    Debug.Write(ex);
                }
            }
            
            try
            {
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
                        throw new BackupException("Authentication failed");
                    }
                    throw;
                }
            }
            return oneDriveClient;
        }

        public async Task LogoutAsync() {
            await oneDriveClient.SignOutAsync();
        }
    }
}