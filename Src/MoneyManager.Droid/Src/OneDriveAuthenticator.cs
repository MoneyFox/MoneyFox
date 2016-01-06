using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Foundation.Interfaces;
using Xamarin.Auth;

namespace MoneyManager.Droid
{
    public class OneDriveAuthenticator : IOneDriveAuthenticator
    {
        private const string MSA_CLIENT_ID = "[ID]";

        private readonly IDialogService dialogService;
        private readonly string[] scopes = { "onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly" };
        private const string RETURN_URL = @"https://login.live.com/oauth20_desktop.srf";

        private IOneDriveClient oneDriveClient;

        public OneDriveAuthenticator(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public async Task<IOneDriveClient> LoginAsync()
        {
            if (oneDriveClient == null)
            {
                var requestUriStringBuilder = new StringBuilder();
                requestUriStringBuilder.Append("https://login.live.com/oauth20_authorize.srf");
                requestUriStringBuilder.AppendFormat("?{0}={1}", Constants.Authentication.RedirectUriKeyName, RETURN_URL);
                requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ClientIdKeyName, MSA_CLIENT_ID);
                requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ScopeKeyName,
                    string.Join("%20", scopes));
                requestUriStringBuilder.AppendFormat("&{0}={1}", Constants.Authentication.ResponseTypeKeyName,
                    Constants.Authentication.TokenResponseTypeValueName);

                var auth = new OAuth2Authenticator(
                    clientId: MSA_CLIENT_ID,
                    scope: string.Join(",", scopes),
                    authorizeUrl: new Uri(requestUriStringBuilder.ToString()),
                    redirectUrl: new Uri(RETURN_URL));


                var appConfig = new AppConfig();

                auth.Completed += (sender, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                    }
                };

                try
                {
                    var intent = auth.GetUI(Application.Context);
                    intent.SetFlags(ActivityFlags.NewTask);

                    Application.Context.StartActivity(intent);
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }
                return new OneDriveClient(appConfig);
            }
            return new OneDriveClient(new AppConfig());
        }
    }
}