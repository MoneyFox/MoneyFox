using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using MoneyFox.BusinessLogic.Extensions;
using MoneyFox.Foundation.Constants;
using MoneyFox.ServiceLayer.Interfaces;
using PCLAppConfig;
using Xamarin.Auth;

namespace MoneyFox.Droid.OneDriveAuth
{
    public class DroidAuthenticationProvider : ICustomAuthenticationProvider
    {
        private OAuth2Authenticator authenticator;

        /// <summary>
        ///     Authenticates the user with the service and saves the refresh token.
        /// </summary>
        /// <param name="request">Http request message</param>
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            authenticator = new OAuth2Authenticator(ConfigurationManager.AppSettings["MsaClientId"],
                                                    ConfigurationManager.AppSettings["MsaClientSecret"],
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
                    // pass access_token to the onedrive sdk
                    accessToken = result[ServiceConstants.ACCESS_TOKEN];
                    
                    // add refresh token to the password vault to enable future silent login
                    new ProtectedData().Protect(ServiceConstants.REFRESH_TOKEN, result[ServiceConstants.REFRESH_TOKEN]);
                }
            }
            else
            {
                accessToken = await authenticator.RequestRefreshTokenAsync(refreshToken);
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        }

        private Task<IDictionary<string, string>> ShowWebView()
        {
            var tcs = new TaskCompletionSource<IDictionary<string, string>>();

            authenticator.Completed += (sender, eventArgs) => 
            {
                tcs.SetResult(eventArgs.IsAuthenticated ? eventArgs.Account.Properties : null);
            };

            var intent = authenticator.GetUI(Application.Context);
            intent.SetFlags(ActivityFlags.NewTask);

            Application.Context.StartActivity(intent);

            return tcs.Task;
        }

        /// <summary>
        ///     Removes the saved refresh token from the cache.
        /// </summary>
        public void Logout()
        {
            new ProtectedData().Remove(ServiceConstants.REFRESH_TOKEN);

            authenticator = new OAuth2Authenticator(ConfigurationManager.AppSettings["MsaClientId"],
                string.Empty,
                new Uri(ServiceConstants.LOGOUT_URL),
                new Uri(ServiceConstants.RETURN_URL));
        }
    }
}