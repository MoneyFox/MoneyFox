using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Droid
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private const string RETURN_URL = @"https://login.live.com/oauth20_desktop.srf";

        private readonly IDialogService dialogService;
        private readonly string[] scopes = {"onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly"};

        private IOneDriveClient oneDriveClient;

        public OneDriveAuthenticator(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public async Task<IOneDriveClient> LoginAsync()
        {
            if (oneDriveClient == null)
            {
                oneDriveClient = OneDriveClient.GetMicrosoftAccountClient(
                    OneDriveAuthenticationConstants.MSA_CLIENT_ID,
                    RETURN_URL,
                    scopes, OneDriveAuthenticationConstants.MSA_SECRET,
                    null, null,
                    new CustomServiceInfoProvider());
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