using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Droid
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private const string MSA_CLIENT_ID = "000000004016F96F";
        private const string MSA_SECRET = "eBl-PFgd-bBANvHaPKkajuTeYUCXo52Z";

        private readonly IDialogService dialogService;
        private readonly string[] scopes = { "onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly" };
        private const string RETURN_URL = @"https://login.live.com/oauth20_desktop.srf";

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
                    MSA_CLIENT_ID,
                    RETURN_URL,
                    scopes, MSA_SECRET, null, null, 
                    new ServiceInfoProvider(new AndroidAuthenticationProvider(new MicrosoftAccountServiceInfo())));
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
            } catch (OneDriveException exception)
            {
                // Swallow authentication cancelled exceptions
                if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString()))
                {
                    if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString()))
                    {
                        await dialogService.ShowMessage(
                            "Authentication failed",
                            "Authentication failed");
                    } else
                    {
                        throw;
                    }
                }
            }
            return oneDriveClient;
        }
    }
}