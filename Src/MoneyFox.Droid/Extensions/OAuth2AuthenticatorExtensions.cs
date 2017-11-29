using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Foundation.Constants;
using Xamarin.Auth;

namespace MoneyFox.Droid.Extensions
{
    public static class OAuth2AuthenticatorExtensions
    {
        ///  <summary>
        ///  	Method that requests a new access token based on an initial refresh token
        ///  </summary>
        /// <param name="authenticator">Authenticator object</param>
        /// <param name="refreshToken">Refresh token, typically from the <see cref="AccountStore"/>'s refresh_token property</param>
        ///  <returns>Time in seconds the refresh token expires in</returns>
        public static Task<string> RequestRefreshTokenAsync(this OAuth2Authenticator authenticator, string refreshToken)
        {
            var queryValues = new Dictionary<string, string>
            {
                {"refresh_token", refreshToken},
                {"client_id", authenticator.ClientId},
                {"grant_type", "refresh_token"}
            };

            if (!string.IsNullOrEmpty(authenticator.ClientSecret))
            {
                queryValues["client_secret"] = authenticator.ClientSecret;
            }

            return authenticator.RequestAccessTokenAsync(queryValues)
                                .ContinueWith(result => result.Result[ServiceConstants.ACCESS_TOKEN]);
        }
    }
}