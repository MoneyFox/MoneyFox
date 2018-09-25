using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.iOS.Authentication
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        public Task<IGraphServiceClient> LoginAsync()
        {
            try
            {
                var tcs = new TaskCompletionSource<IGraphServiceClient>();

                tcs.SetResult(new GraphServiceClient(ServiceConstants.BASE_URL, new IosAuthenticationProvider()));

                return tcs.Task;
            } catch (Exception)
            {
                throw new BackupException("Authentication Failed");
            }
        }

        public Task LogoutAsync()
        {
            new IosAuthenticationProvider().Logout();
            return Task.CompletedTask;
        }
    }
}
