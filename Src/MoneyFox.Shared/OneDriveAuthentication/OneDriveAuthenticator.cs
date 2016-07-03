using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Shared.OneDriveAuthentication {

    public class OneDriveAuthenticator : IOneDriveAuthenticator {
        private readonly IDialogService dialogService;
        private readonly ServiceInfoProvider serviceInfoProvider;

        private IOneDriveClient oneDriveClient;

        public OneDriveAuthenticator(IDialogService dialogService, ServiceInfoProvider serviceInfoProvider) {
            this.dialogService = dialogService;
            this.serviceInfoProvider = serviceInfoProvider;
        }

        public async Task<IOneDriveClient> LoginAsync() {
            if (oneDriveClient == null) {
                oneDriveClient = OneDriveClient.GetMicrosoftAccountClient(
                    ServiceConstants.MSA_CLIENT_ID,
                    ServiceConstants.RETURN_URL,
                    ServiceConstants.Scopes,
                    ServiceConstants.MSA_CLIENT_SECRET,
                    null, null,
                    serviceInfoProvider);
                try {
                    await oneDriveClient.AuthenticateAsync();
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex);
                }
            }

            try {
                if (!oneDriveClient.IsAuthenticated) {
                    await oneDriveClient.AuthenticateAsync();
                }

                return oneDriveClient;
            }
            catch (OneDriveException exception) {
                // Swallow authentication cancelled exceptions
                if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString())) {
                    if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString())) {
                        await dialogService.ShowMessage(
                            "Authentication failed",
                            "Authentication failed");
                    }
                    else {
                        throw;
                    }
                }
            }
            return oneDriveClient;
        }
    }
}