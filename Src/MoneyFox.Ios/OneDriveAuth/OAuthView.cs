using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MoneyFox.Shared.Constants;
using MvvmCross.iOS.Views;
using Xamarin.Auth;

namespace MoneyFox.Ios.OneDriveAuth {
    public class OAuthView : MvxViewController {

        public Task<IDictionary<string, string>> ShowWebView() {
            var tcs = new TaskCompletionSource<IDictionary<string, string>>();

            var auth = new OAuth2Authenticator(ServiceConstants.MSA_CLIENT_ID,
                ServiceConstants.MSA_CLIENT_SECRET,
                string.Join(",", ServiceConstants.Scopes),
                new Uri(GetAuthorizeUrl()),
                new Uri(ServiceConstants.RETURN_URL),
                new Uri(ServiceConstants.TOKEN_URL));

            auth.Completed += (sender, eventArgs) => {
                DismissViewController(true, null);

                if (eventArgs.IsAuthenticated) {
                    AccountStore.Create().Save(eventArgs.Account, ServiceConstants.KEY_STORE_TAG_ONEDRIVE);
                    tcs.SetResult(eventArgs.Account.Properties);
                }
            };

            PresentViewController(auth.GetUI(), true, null);

            return tcs.Task;
        }

        private string GetAuthorizeUrl() {
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