using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using Xamarin.Auth;
using Constants = Microsoft.OneDrive.Sdk.Constants;

namespace MoneyFox.Ios.OneDriveAuth {
    public class IosAuthenticationProvider : AuthenticationProvider {

        public IosAuthenticationProvider(ServiceInfo serviceInfo) : base(serviceInfo) {
        }
        
        protected override async Task<AccountSession> GetAuthenticationResultAsync() {
            var sessionFromCache = await GetSessionFromCache();

            if (sessionFromCache != null) {
                return sessionFromCache;
            }

            return new AccountSession(await new OAuthView().ShowWebView(), ServiceInfo.AppId,
                AccountType.MicrosoftAccount) {
                    CanSignOut = true
                };
        }

        /// <summary>
        ///     Tries to get an account session from the cache or via the refresh token.
        /// </summary>
        /// <returns>AccountSession created via the refresh token.</returns>
        private async Task<AccountSession> GetSessionFromCache() {
            var accounts = AccountStore.Create().FindAccountsForService(ServiceConstants.KEY_STORE_TAG_ONEDRIVE).ToList();

            if (accounts.Any()) {
                var accountValues = accounts.FirstOrDefault()?.Properties;

                if (accountValues == null) {
                    return null;
                }

                string refreshToken;
                if (accountValues.TryGetValue(Constants.Authentication.RefreshTokenKeyName, out refreshToken)) {
                    return await RefreshAccessTokenAsync(refreshToken);
                }
            }
            return null;
        }

        public override Task SignOutAsync() {
            throw new NotImplementedException();
        }
    }
}