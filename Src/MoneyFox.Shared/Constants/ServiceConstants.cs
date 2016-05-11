namespace MoneyFox.Shared.Constants
{
    public class ServiceConstants
    {
        public const string HOCKEY_APP_DROID_ID = "6ae4ef16925140c3b2a14b8ffeeba8fb";

        public const string HOCKEY_APP_WINDOWS_ID = "ac915a3736f5436ab85b5a5617838bc8";

        public const string MSA_CLIENT_ID = "000000004416B470";

        public const string MSA_CLIENT_SECRET = "YoWyKydsjQTLEEjklZO049M38BBi8X5k";

        /// <summary>
        ///     Return url for the OneDrive authentication
        /// </summary>
        public const string RETURN_URL = "https://login.live.com/oauth20_desktop.srf";

        /// <summary>
        ///     Authentication url for the OneDrive authentication
        /// </summary>
        public const string AUTHENTICATION_URL = "https://login.live.com/oauth20_authorize.srf";

        /// <summary>
        ///     The Token URL is used to retrieve a access token in the code flow oauth
        /// </summary>
        public const string TOKEN_URL = "https://login.live.com/oauth20_token.srf";

        /// <summary>
        ///     Scopes for OneDrive access
        /// </summary>
        public static string[] Scopes = { "onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly" };
    }
}
