using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using MoneyFox.Business.Extensions;
using MoneyFox.Foundation.Constants;
using UIKit;
using Xamarin.Auth;

namespace MoneyFox.iOS.Authentication {
    public class IosAuthenticationProvider : IAuthenticationProvider
    {
        private OAuth2Authenticator authenticator;

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            authenticator = new OAuth2Authenticator(ServiceConstants.MSA_CLIENT_ID,
                ServiceConstants.MSA_CLIENT_SECRET,
                string.Join(",", ServiceConstants.Scopes),
                new Uri(ServiceConstants.AUTHENTICATION_URL),
                new Uri(ServiceConstants.RETURN_URL),
                new Uri(ServiceConstants.TOKEN_URL));

            var protectedData = new ProtectedData();
            var accessToken = string.Empty;
            var refreshToken = protectedData.Unprotect(ServiceConstants.REFRESH_TOKEN);
            if (string.IsNullOrEmpty(refreshToken))
            {
                var result = await ShowWebView();
                if (result != null)
                {
                    UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewController(true, null);
                    // pass access_token to the onedrive sdk
                    accessToken = result[ServiceConstants.ACCESS_TOKEN];

                    // add refresh token to the password vault to enable future silent login
                    protectedData.Protect(ServiceConstants.REFRESH_TOKEN, result[ServiceConstants.REFRESH_TOKEN]);
                }
            } else
            {
                accessToken = await authenticator.RequestRefreshTokenAsync(refreshToken);
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        }

        private Task<IDictionary<string, string>> ShowWebView()
        {
            var tcs = new TaskCompletionSource<IDictionary<string, string>>();

            authenticator.Completed +=
                (sender, eventArgs) =>
                {
                    tcs.SetResult(eventArgs.IsAuthenticated ? eventArgs.Account.Properties : null);
                };

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(authenticator.GetUI(), true, null);

            return tcs.Task;
        }

        /// <summary>
        ///     Removes the saved refresh token from the cache.
        /// </summary>
        public void Logout()
        {
            new ProtectedData().Remove(ServiceConstants.REFRESH_TOKEN);

            authenticator = new OAuth2Authenticator(ServiceConstants.MSA_CLIENT_ID,
                string.Empty,
                new Uri(ServiceConstants.LOGOUT_URL),
                new Uri(ServiceConstants.RETURN_URL));
        }
    }
}