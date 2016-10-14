using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.Graph;
using MoneyFox.Shared.Constants;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Xamarin.Auth;

namespace MoneyFox.Droid.OneDriveAuth
{
    public class DroidAuthenticationProvider : IAuthenticationProvider
    {
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var protectedData = new ProtectedData();
            var token = protectedData.Unprotect(ServiceConstants.ACCESS_TOKEN);
            if (string.IsNullOrEmpty(token))
            {
                var result = await ShowWebView();
                if (result != null)
                {
                    token = result[ServiceConstants.ACCESS_TOKEN];
                    new ProtectedData().Protect(ServiceConstants.ACCESS_TOKEN, token);
                }
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        private Task<IDictionary<string, string>> ShowWebView()
        {
            var tcs = new TaskCompletionSource<IDictionary<string, string>>();

            var auth = new OAuth2Authenticator(ServiceConstants.MSA_CLIENT_ID,
                string.Join(",", ServiceConstants.Scopes),
                new Uri(GetAuthorizeUrl()),
                new Uri(ServiceConstants.RETURN_URL));

            auth.Completed +=
                (sender, eventArgs) =>
                {
                    tcs.SetResult(eventArgs.IsAuthenticated ? eventArgs.Account.Properties : null);
                };

            Debug.Write("Request Parameter");
            foreach (var authRequestParameter in auth.RequestParameters)
            {
                Debug.Write(authRequestParameter.Key);
                Debug.WriteLine(authRequestParameter.Value);
            }

            var intent = auth.GetUI(Application.Context);
            intent.SetFlags(ActivityFlags.NewTask);

            Application.Context.StartActivity(intent);

            return tcs.Task;
        }

        private string GetAuthorizeUrl()
        {
            var requestUriStringBuilder = new StringBuilder();
            requestUriStringBuilder.Append(ServiceConstants.AUTHENTICATION_URL);
            requestUriStringBuilder.AppendFormat("?{0}={1}", ServiceConstants.REDIRECT_URI,
                ServiceConstants.RETURN_URL);
            requestUriStringBuilder.AppendFormat("&{0}={1}", ServiceConstants.CLIENT_ID,
                ServiceConstants.MSA_CLIENT_ID);
            requestUriStringBuilder.AppendFormat("&{0}={1}", ServiceConstants.SCOPE,
                string.Join("%20", ServiceConstants.Scopes));
            requestUriStringBuilder.AppendFormat("&{0}={1}", ServiceConstants.RESPONSE_TYPE, ServiceConstants.CODE);

            return requestUriStringBuilder.ToString();
        }
    }
}