using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using MoneyFox.Shared.Constants;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Xamarin.Auth;
using System;
using System.Net.Http;
using System.Text;
using Microsoft.Graph;

namespace MoneyFox.Droid.OneDriveAuth {

    public class DroidAuthenticationProvider :  IAuthenticationProvider 
    {
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public async Task GetAuthenticationResultAsync() 
        {
            //var sessionFromCache = await GetSessionFromCache();

            //if (sessionFromCache != null) {
            //    return sessionFromCache;
            //}

            //return new AccountSession(await ShowWebView(), ServiceInfo.AppId,
            //    AccountType.MicrosoftAccount) {
            //    CanSignOut = true
            //};
        }

        /// <summary>
        ///     Tries to get an account session from the cache or via the refresh token.
        /// </summary>
        /// <returns>AccountSession created via the refresh token.</returns>
        private async Task GetSessionFromCache() 
        {
            var accounts = AccountStore.Create(Application.Context).FindAccountsForService(ServiceConstants.KEY_STORE_TAG_ONEDRIVE).ToList();

            if (accounts.Any()) 
            {
                var accountValues = accounts.FirstOrDefault()?.Properties;

                if (accountValues == null) 
                {
                }

                string refreshToken;
                //if (accountValues.TryGetValue(Constants.Authentication.RefreshTokenKeyName, out refreshToken)) 
                //{
                //    return await RefreshAccessTokenAsync(refreshToken);
                //}
            }
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
            requestUriStringBuilder.AppendFormat("?{0}={1}", "redirect_uri",
                ServiceConstants.RETURN_URL);
            requestUriStringBuilder.AppendFormat("&{0}={1}", "client_id",
                ServiceConstants.MSA_CLIENT_ID);
            requestUriStringBuilder.AppendFormat("&{0}={1}", "scope",
                string.Join("%20", ServiceConstants.Scopes));
            requestUriStringBuilder.AppendFormat("&{0}={1}", "response_type", "code");

            return requestUriStringBuilder.ToString();
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            await ShowWebView();
        }
    }
}