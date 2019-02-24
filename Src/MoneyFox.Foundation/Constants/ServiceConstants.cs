#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace MoneyFox.Foundation.Constants
{
    /// <summary>
    ///     Contains constants for the services used in the app.
    /// </summary>
    public static class ServiceConstants
    {
        /// <summary>
        ///     Return URL for the OneDrive authentication
        /// </summary>
        public const string RETURN_URL = "https://login.live.com/oauth20_desktop.srf";

        /// <summary>
        ///     Returns the base URL of the OneDrive Service
        /// </summary>
        public const string BASE_URL = "https://api.onedrive.com/v1.0";

        /// <summary>
        ///     Authentication URL for the OneDrive authentication
        /// </summary>
        public const string AUTHENTICATION_URL = "https://login.live.com/oauth20_authorize.srf";

        /// <summary>
        ///     Logout URL for the OneDrive authentication
        /// </summary>
        public const string LOGOUT_URL = "https://login.live.com/oauth20_logout.srf";

        /// <summary>
        ///     The Token URL is used to retrieve a access token in the code flow OAUTH
        /// </summary>
        public const string TOKEN_URL = "https://login.live.com/oauth20_token.srf";

        /// <summary>
        ///     String constant for the access token.
        /// </summary>
        public const string ACCESS_TOKEN = "access_token";
        
        /// <summary>
        ///     String constant for the refresh token.
        /// </summary>
        public const string REFRESH_TOKEN = "refresh_token";

        /// <summary>
        ///     String constant for the code.
        /// </summary>
        public const string CODE = "code";

        /// <summary>
        ///     Scopes for OneDrive access
        /// </summary>
        public static readonly string[] Scopes = {"onedrive.readwrite", "wl.offline_access", "wl.signin"};

        /// <summary>
        ///     Maximum number of attempts to sync the database
        /// </summary>
        public const int SYNC_ATTEMPTS = 2;

        /// <summary>
        ///     The amount of time to wait for the OneDrive backup to be completed
        /// </summary>
        public const int BACKUP_OPERATION_TIMEOUT = 10000;

        /// <summary>
        ///     The amount of time to wait before retrying to sync
        /// </summary>
        public const int BACKUP_REPEAT_DELAY = 2000;
    }
}