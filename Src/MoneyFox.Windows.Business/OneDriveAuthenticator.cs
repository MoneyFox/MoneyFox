using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Windows.Business
{
    /// <inheritdoc />
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private readonly bool isBackground = false;

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public OneDriveAuthenticator()
        {
        }

        /// <summary>
        ///     Constructor to set if in Background or not.
        /// </summary>
        /// <param name="isBackground">Defines if the authenticator is used in Background.</param>
        public OneDriveAuthenticator(bool isBackground)
        {
            this.isBackground = isBackground;
        }

        /// <inheritdoc />
        public async Task<IOneDriveClient> LoginAsync()
        {
            try
            {
                var msaAuthenticationProvider = new OnlineIdAuthenticationProvider(ServiceConstants.Scopes,
                    isBackground
                        ? OnlineIdAuthenticationProvider.PromptType.DoNotPrompt
                        : OnlineIdAuthenticationProvider.PromptType.PromptIfNeeded);

                await msaAuthenticationProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                return new OneDriveClient(ServiceConstants.BASE_URL, msaAuthenticationProvider);
            }
            catch (ServiceException serviceException)
            {
                Debug.WriteLine(serviceException);
                throw new BackupException("Authentication Failed with Graph.ServiceException", serviceException);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new BackupException("Authentication Failed", ex);
            }
        }

        /// <inheritdoc />
        public async Task LogoutAsync()
        {
            await new OnlineIdAuthenticationProvider(ServiceConstants.Scopes)
                .SignOutAsync();
        }
    }
}