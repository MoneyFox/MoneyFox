using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Ios.OneDriveAuth {
    public class OneDriveAuthenticator : IOneDriveAuthenticator {
        private readonly IDialogService dialogService;
        private IOneDriveClient oneDriveClient;

        public OneDriveAuthenticator(IDialogService dialogService) {
            this.dialogService = dialogService;
        }

        public async Task<IOneDriveClient> LoginAsync() {
            if (oneDriveClient == null) {
                oneDriveClient = OneDriveClient.GetMicrosoftAccountClient(
                    ServiceConstants.MSA_CLIENT_ID,
                    ServiceConstants.RETURN_URL,
                    ServiceConstants.Scopes,
                    ServiceConstants.MSA_CLIENT_SECRET,
                    null, null,
                    new IosServiceInfoProvider());
                try {
                    await oneDriveClient.AuthenticateAsync();
                } catch (Exception ex) {
                    Debug.Write(ex);
                }
            }

            try {
                if (!oneDriveClient.IsAuthenticated) {
                    await oneDriveClient.AuthenticateAsync();
                }

                return oneDriveClient;
            } catch (OneDriveException exception) {
                // Swallow authentication cancelled exceptions
                if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString())) {
                    if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString())) {
                        await dialogService.ShowMessage(
                            "Authentication failed",
                            "Authentication failed");
                    } else {
                        throw;
                    }
                }
            }
            return oneDriveClient;
        }
    }
}