using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Droid.OneDriveAuth
{
    /// <inheritdoc />
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        /// <inheritdoc />
        public Task<IOneDriveClient> LoginAsync()
        {
            try
            {
                var tcs = new TaskCompletionSource<IOneDriveClient>();

                tcs.SetResult(new OneDriveClient(ServiceConstants.BASE_URL, new DroidAuthenticationProvider()));

                return tcs.Task;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                throw new BackupException("Authentication Failed");
            }
        }

        /// <inheritdoc />
        public Task LogoutAsync()
        {
            new DroidAuthenticationProvider().Logout();
            return Task.CompletedTask;
        }
    }
}