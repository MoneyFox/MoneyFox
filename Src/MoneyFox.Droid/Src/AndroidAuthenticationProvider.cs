using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Xamarin.Auth;
using Constants = Microsoft.OneDrive.Sdk.Constants;

namespace MoneyFox.Droid
{
    public class AndroidAuthenticationProvider : AuthenticationProvider
    {
        private IDictionary<string, string> authenticationResponseValues;
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

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

        public const string ONEDRIVE_KEY = "OneDrive";

        private Task<bool> ShowWebView()
        {
            var tcs = new TaskCompletionSource<bool>();

            var accounts = AccountStore.Create(Application.Context).FindAccountsForService(ONEDRIVE_KEY).ToList();

            if (accounts.Any())
            {
                authenticationResponseValues = accounts.FirstOrDefault()?.Properties;
                tcs.SetResult(true);
                return tcs.Task;
            }


            var auth = new OAuth2Authenticator(OneDriveAuthenticationConstants.MSA_CLIENT_ID,
                string.Join(",", OneDriveAuthenticationConstants.Scopes), new Uri(GetAuthorizeUrl()),
                new Uri(OneDriveAuthenticationConstants.RETURN_URL));

            auth.Completed += (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                { 
                    OAuthErrorHandler.ThrowIfError(eventArgs.Account.Properties);
                    authenticationResponseValues = eventArgs.Account.Properties;
                    AccountStore.Create(Application.Context).Save(eventArgs.Account, ONEDRIVE_KEY);
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