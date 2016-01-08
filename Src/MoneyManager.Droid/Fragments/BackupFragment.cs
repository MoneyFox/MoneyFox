using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using MvvmCross.Plugins.Messenger;
using Xamarin.Auth;

namespace MoneyManager.Droid.Fragments
{
    public class BackupFragment : MvxFragment
    {
        public new BackupViewModel ViewModel
        {
            get { return (BackupViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.BackupLayout, null);

            ShowWebView();

            return view;
        }

        private const string MSA_CLIENT_ID = "000000004016F96F";

        private readonly string[] scopes = { "onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly" };
        private const string RETURN_URL = "https://login.live.com/oauth20_desktop.srf";


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
                OAuthErrorHandler.ThrowIfError(eventArgs.Account.Properties);
                var message = new TempMessage
                {
                    AuthenticationResponseValues = eventArgs.Account.Properties
                };

                Mvx.RegisterType(() => message);
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
    }
}