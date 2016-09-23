using System;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Droid.OneDriveAuth
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private IOneDriveClient oneDriveClient;

        public async Task<IOneDriveClient> LoginAsync()
        {
            try
            {
                return new OneDriveClient(ServiceConstants.BASE_URL, new DroidAuthenticationProvider());
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