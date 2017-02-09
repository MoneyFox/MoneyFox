using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using MoneyFox.Business.Extensions;
using UIKit;
using Xamarin.Auth;
using MoneyFox.Ios.OneDriveAuth;

namespace MoneyFox.Ios
{
	public class IosAuthenticationProvider : ICustomAuthenticationProvider
	{
		private OAuth2Authenticator authenticator;

		public async Task AuthenticateRequestAsync(HttpRequestMessage request)
		{
			authenticator = new OAuth2Authenticator(ServiceConstants.MSA_CLIENT_ID,
				ServiceConstants.MSA_CLIENT_SECRET,
				string.Join(",", ServiceConstants.Scopes),
				new Uri(ServiceConstants.AUTHENTICATION_URL),
				new Uri(ServiceConstants.RETURN_URL),
				new Uri(ServiceConstants.TOKEN_URL));

			var protectedData = new ProtectedData();
			var accessToken = string.Empty;
			var refreshToken = protectedData.Unprotect(ServiceConstants.REFRESH_TOKEN);

			if (string.IsNullOrEmpty(refreshToken))
			{
				var authView = new OAuthView();
				var result = await authView.ShowWebView();
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

		public void Logout()
		{
			new ProtectedData().Remove(ServiceConstants.REFRESH_TOKEN);

			authenticator = new OAuth2Authenticator(ServiceConstants.MSA_CLIENT_ID,
				string.Empty,
				new Uri(ServiceConstants.LOGOUT_URL),
				new Uri(ServiceConstants.RETURN_URL));
		}
	}
}