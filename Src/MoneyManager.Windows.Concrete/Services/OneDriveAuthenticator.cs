using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows.Concrete.Services
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private const string MSA_CLIENT_ID = "https://login.live.com/oauth20_desktop.srf";
        private const string MSA_RETURN_URL = "https://login.live.com/oauth20_desktop.srf";

        private static readonly string[] Scopes = {"onedrive.readwrite", "wl.offline_access", "wl.signin"};

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
                oneDriveClient = OneDriveClientExtensions.GetUniversalClient(Scopes);

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
            catch (Exception ec)
            {
                Debug.WriteLine(ec.Message);
            }
            return oneDriveClient;
        }
    }
}