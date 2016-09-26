using System;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Ios.OneDriveAuth
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        public async Task<IOneDriveClient> LoginAsync()
        {
            try
            {
                return new OneDriveClient(ServiceConstants.BASE_URL, new IosAuthenticationProvider());
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