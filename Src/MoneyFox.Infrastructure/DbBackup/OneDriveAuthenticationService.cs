namespace MoneyFox.Infrastructure.DbBackup
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Common.Exceptions;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using NLog;
    using Logger = NLog.Logger;

    internal sealed class OneDriveAuthenticationService : IOneDriveAuthenticationService
    {
        private const string ERROR_CODE_CANCELED = "authentication_canceled";

        private readonly string[] scopes = { "Files.ReadWrite", "User.ReadBasic.All" };

        private readonly IPublicClientApplication publicClientApplication;
        private readonly IGraphClientFactory graphClientFactory;

        public OneDriveAuthenticationService(IPublicClientApplication publicClientApplication, IGraphClientFactory graphClientFactory)
        {
            this.publicClientApplication = publicClientApplication;
            this.graphClientFactory = graphClientFactory;
        }

        public async Task<GraphServiceClient> CreateServiceClient(CancellationToken cancellationToken = default)
        {
            IEnumerable<IAccount> accounts = await publicClientApplication.GetAccountsAsync();
            try
            {
                var firstAccount = accounts.FirstOrDefault();
                var authResult = firstAccount == null
                    ? await AcquireInteractive()
                    : await publicClientApplication.AcquireTokenSilent(scopes: scopes, account: firstAccount).ExecuteAsync(cancellationToken);

                return graphClientFactory.CreateClient(authResult);
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    throw new BackupOperationCanceledException();
                }

                throw;
            }
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                List<IAccount> accounts = (await publicClientApplication.GetAccountsAsync()).ToList();
                while (accounts.Any())
                {
                    await publicClientApplication.RemoveAsync(accounts.FirstOrDefault());
                    accounts = (await publicClientApplication.GetAccountsAsync()).ToList();
                }
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {

                    throw new BackupOperationCanceledException();
                }

                throw;
            }
            catch (Exception ex)
            {
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        private async Task<AuthenticationResult> AcquireInteractive()
        {
            return await publicClientApplication.AcquireTokenInteractive(scopes)
                .WithUseEmbeddedWebView(true)
                .WithParentActivityOrWindow(ParentActivityWrapper.ParentActivity) // this is required for Android
                .ExecuteAsync();
        }
    }

}
