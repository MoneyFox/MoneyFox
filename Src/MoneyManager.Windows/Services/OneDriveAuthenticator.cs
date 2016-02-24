using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows.Services
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private readonly IDialogService dialogService;

        private IOneDriveClient oneDriveClient;

        public OneDriveAuthenticator(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public async Task<IOneDriveClient> LoginAsync()
        {
            if (oneDriveClient == null)
            {
                oneDriveClient = OneDriveClientExtensions.GetUniversalClient(OneDriveAuthenticationConstants.Scopes);
                await oneDriveClient.AuthenticateAsync();
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
                        await dialogService.ShowMessage(
                            "Authentication failed",
                            "Authentication failed");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return oneDriveClient;
        }
    }
}