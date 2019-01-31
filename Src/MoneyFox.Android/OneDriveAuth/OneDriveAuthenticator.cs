using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;

namespace MoneyFox.Droid.OneDriveAuth
{
    /// <inheritdoc />
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        /// <inheritdoc />
        public Task<IGraphServiceClient> LoginAsync()
        {
            try
            {
                var tcs = new TaskCompletionSource<IGraphServiceClient>();

                tcs.SetResult(new GraphServiceClient(ServiceConstants.BASE_URL, new DroidAuthenticationProvider()));

                return tcs.Task;
            }
            catch (Exception)
            {
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