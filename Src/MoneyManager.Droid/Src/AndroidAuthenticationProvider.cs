using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Cirrious.CrossCore;
using Microsoft.OneDrive.Sdk;
using Xamarin.Auth;

namespace MoneyManager.Droid
{
    public class AndroidAuthenticationProvider : AuthenticationProvider
    {
        private const string MSA_CLIENT_ID = "000000004016F96F";
        private const string RETURN_URL = "https://login.live.com/oauth20_desktop.srf";

        private readonly string[] scopes = {"onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly"};

        private IDictionary<string, string> authenticationResponseValues;

        public AndroidAuthenticationProvider(ServiceInfo serviceInfo) : base(serviceInfo)
        {
        }

        protected override async Task<AccountSession> GetAuthenticationResultAsync()
        {
            await ShowWebView();
            return new AccountSession(authenticationResponseValues, ServiceInfo.AppId,
                AccountType.MicrosoftAccount)
            {
                CanSignOut = true
            };
        }

        public override Task SignOutAsync()
        {
            throw new NotImplementedException();
        }

        private Task<bool> ShowWebView()
        {
            var tcs = new TaskCompletionSource<bool>();

            var auth = new OAuth2Authenticator(MSA_CLIENT_ID, string.Join(",", scopes), new Uri(GetAuthorizeUrl()),
                new Uri(RETURN_URL));

            auth.Completed += (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    OAuthErrorHandler.ThrowIfError(eventArgs.Account.Properties);
                    authenticationResponseValues = eventArgs.Account.Properties;
                    tcs.SetResult(true);
                }
            };

            var intent = auth.GetUI(Application.Context);
            intent.SetFlags(ActivityFlags.NewTask);

            Application.Context.StartActivity(intent);

            return tcs.Task;
        }

        private string GetAuthorizeUrl()
        {
            var requestUriStringBuilder = new StringBuilder();
            requestUriStringBuilder.Append("https://login.live.com/oauth20_authorize.srf");
            requestUriStringBuilder.AppendFormat("?{0}={1}", Constants.Authentication.RedirectUriKeyName, RETURN_URL);
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ClientIdKeyName, MSA_CLIENT_ID);
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ScopeKeyName,
                string.Join("%20", scopes));
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ResponseTypeKeyName,
                Constants.Authentication.TokenResponseTypeValueName);

            return requestUriStringBuilder.ToString();
        }
    }
}