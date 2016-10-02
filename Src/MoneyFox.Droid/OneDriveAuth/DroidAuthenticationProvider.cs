using Android.App;
using Android.Content;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Constants = Microsoft.OneDrive.Sdk.Constants;

namespace MoneyFox.Droid.OneDriveAuth {

    public class DroidAuthenticationProvider : AuthenticationProvider 
    {

        public DroidAuthenticationProvider(ServiceInfo serviceInfo) : base(serviceInfo) {}

        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        protected override async Task<AccountSession> GetAuthenticationResultAsync() 
        {
            var sessionFromCache = await GetSessionFromCache();

            if (sessionFromCache != null) {
                return sessionFromCache;
            }

            return new AccountSession(await ShowWebView(), ServiceInfo.AppId,
                AccountType.MicrosoftAccount) {
                CanSignOut = true
            };
        }

        /// <summary>
        ///     Tries to get an account session from the cache or via the refresh token.
        /// </summary>
        /// <returns>AccountSession created via the refresh token.</returns>
        private async Task<AccountSession> GetSessionFromCache() 
        {
            var accounts = AccountStore.Create(Application.Context).FindAccountsForService(ServiceConstants.KEY_STORE_TAG_ONEDRIVE).ToList();

            if (accounts.Any()) 
            {
                var accountValues = accounts.FirstOrDefault()?.Properties;

                if (accountValues == null) 
                {
                    return null;
                }

                string refreshToken;
                if (accountValues.TryGetValue(Constants.Authentication.RefreshTokenKeyName, out refreshToken)) 
                {
                    return await RefreshAccessTokenAsync(refreshToken);
                }
            }
            return null;
        }

        public override async Task SignOutAsync()
        {
            var store = AccountStore.Create(Application.Context);
            await store.DeleteAsync(store.FindAccountsForService(ServiceConstants.KEY_STORE_TAG_ONEDRIVE).FirstOrDefault(),
                ServiceConstants.KEY_STORE_TAG_ONEDRIVE);
        }

        private Task<IDictionary<string, string>> ShowWebView() 
        {
            var tcs = new TaskCompletionSource<IDictionary<string, string>>();

            var auth = new OAuth2Authenticator(ServiceConstants.MSA_CLIENT_ID,
                ServiceConstants.MSA_CLIENT_SECRET,
                string.Join(",", ServiceConstants.Scopes),
                new Uri(GetAuthorizeUrl()),
                new Uri(ServiceConstants.RETURN_URL),
                new Uri(ServiceConstants.TOKEN_URL));

            auth.Completed += (sender, eventArgs) => 
            {
                if (eventArgs.IsAuthenticated) 
                {
                    OAuthErrorHandler.ThrowIfError(eventArgs.Account.Properties);
                    AccountStore.Create(Application.Context).Save(eventArgs.Account, ServiceConstants.KEY_STORE_TAG_ONEDRIVE);
                    tcs.SetResult(eventArgs.Account.Properties);
                }
                tcs.SetResult(null);
            };

            var intent = auth.GetUI(Application.Context);
            intent.SetFlags(ActivityFlags.NewTask);

            Application.Context.StartActivity(intent);

            return tcs.Task;
        }

        private string GetAuthorizeUrl() 
        {
            var requestUriStringBuilder = new StringBuilder();
            requestUriStringBuilder.Append(ServiceConstants.AUTHENTICATION_URL);
            requestUriStringBuilder.AppendFormat("?{0}={1}", Constants.Authentication.RedirectUriKeyName,
                ServiceConstants.RETURN_URL);
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ClientIdKeyName,
                ServiceConstants.MSA_CLIENT_ID);
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ScopeKeyName,
                string.Join("%20", ServiceConstants.Scopes));
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ResponseTypeKeyName,
                Constants.Authentication.CodeKeyName);

            return requestUriStringBuilder.ToString();
        }
    }
}