using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.OneDrive.Sdk;
using Xamarin.Auth;

namespace MoneyManager.Droid
{
    public class AndroidAuthenticationProvider : AuthenticationProvider
    {
        private const string MSA_CLIENT_ID = "000000004016F96F";

        private readonly string[] scopes = { "onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly" };
        private const string RETURN_URL = "https://login.live.com/oauth20_desktop.srf";

        private IDictionary<string, string> authenticationResponseValues;


        public AndroidAuthenticationProvider(ServiceInfo serviceInfo) : base(serviceInfo)
        {
        }

        protected override async Task<AccountSession> GetAuthenticationResultAsync()
        {
            var tcs = new TaskCompletionSource<AccountSession>();

            await Task.Run(() => ShowWebView());
            OAuthErrorHandler.ThrowIfError(authenticationResponseValues);

            tcs.SetResult(new AccountSession(authenticationResponseValues, this.ServiceInfo.AppId, AccountType.MicrosoftAccount) { CanSignOut = true });

            return tcs.Task.Result;
        }

        private void ShowWebView()
        {
            var auth = new OAuth2Authenticator(
                    clientId: MSA_CLIENT_ID,
                    scope: string.Join(",", scopes),
                    authorizeUrl: new Uri(GetAuthorizeUrl()),
                    redirectUrl: new Uri(RETURN_URL));

            var isWaiting = true;

            auth.Completed += (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    OAuthErrorHandler.ThrowIfError(eventArgs.Account.Properties);
                    authenticationResponseValues = eventArgs.Account.Properties;
                }
                isWaiting = false;
            };

            var intent = auth.GetUI(Application.Context);
            intent.SetFlags(ActivityFlags.NewTask);

            Application.Context.StartActivity(intent);

            while (isWaiting)
            {
                Thread.Sleep(500);
            }
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


        public override Task SignOutAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}