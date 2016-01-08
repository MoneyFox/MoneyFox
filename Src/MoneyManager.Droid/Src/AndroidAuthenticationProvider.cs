using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.OneDrive.Sdk;
using Xamarin.Auth;
using System.Diagnostics;
using Cirrious.CrossCore;
using MoneyManager.Droid.Fragments;
using MvvmCross.Plugins.Messenger;

namespace MoneyManager.Droid
{
    public class AndroidAuthenticationProvider : AuthenticationProvider
    {
        private const string MSA_CLIENT_ID = "000000004016F96F";

        private readonly string[] scopes = { "onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly" };
        private const string RETURN_URL = "https://login.live.com/oauth20_desktop.srf";

        public AndroidAuthenticationProvider(ServiceInfo serviceInfo) : base(serviceInfo)
        {
        }

        protected override async Task<AccountSession> GetAuthenticationResultAsync()
        {
            //await Task.Run(() => ShowWebView());

            var temp = Mvx.Resolve<TempMessage>().AuthenticationResponseValues;

            return new AccountSession(temp, this.ServiceInfo.AppId, AccountType.MicrosoftAccount)
            {
                CanSignOut = true
            };
        }

        private void ShowWebView()
        {
            var auth = new OAuth2Authenticator(
                    clientId: MSA_CLIENT_ID,
                    scope: string.Join(",", scopes),
                    authorizeUrl: new Uri(GetAuthorizeUrl()),
                    redirectUrl: new Uri(RETURN_URL));

        
            auth.Completed += SetAccountInfos;

            var intent = auth.GetUI(Application.Context);
            intent.SetFlags(ActivityFlags.NewTask);

            Application.Context.StartActivity(intent);
        }

        private void SetAccountInfos(object sender, AuthenticatorCompletedEventArgs eventArgs)
        {
            if (eventArgs.IsAuthenticated)
            {
                Debug.WriteLine(eventArgs);
                Debug.WriteLine(eventArgs.Account == null ? "IS NULL" : "IS NOT NULL");

                if (eventArgs.Account != null)
                {
                    OAuthErrorHandler.ThrowIfError(eventArgs.Account.Properties);
                    //authenticationResponseValues = eventArgs.Account.Properties;
                }
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