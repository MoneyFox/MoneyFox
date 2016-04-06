using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Foundation;
using Xamarin.Auth;
using Constants = Microsoft.OneDrive.Sdk.Constants;

namespace MoneyManager.Droid
{
    public class AndroidAuthenticationProvider : AuthenticationProvider
    {
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

            var auth = new OAuth2Authenticator(OneDriveAuthenticationConstants.MSA_CLIENT_ID,
                string.Join(",", OneDriveAuthenticationConstants.Scopes), new Uri(GetAuthorizeUrl()),
                new Uri(OneDriveAuthenticationConstants.RETURN_URL));

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
            requestUriStringBuilder.Append(OneDriveAuthenticationConstants.AUTHENTICATION_URL);
            requestUriStringBuilder.AppendFormat("?{0}={1}", Constants.Authentication.RedirectUriKeyName,
                OneDriveAuthenticationConstants.RETURN_URL);
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ClientIdKeyName,
                OneDriveAuthenticationConstants.MSA_CLIENT_ID);
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ScopeKeyName,
                string.Join("%20", OneDriveAuthenticationConstants.Scopes));
            requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ResponseTypeKeyName,
                Constants.Authentication.CodeKeyName);

            return requestUriStringBuilder.ToString();
        }
    }
}