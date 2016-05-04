using Windows.Foundation.Metadata;

namespace MoneyFox.Shared.Constants
{
    /// <summary>
    ///     Contains constant values used for the onedrive authentication
    /// </summary>
    public class BackupConstants
    {
        /// <summary>
        ///     Name of the sqlite database
        /// </summary>
        [Deprecated("Use DB_NAME instead", DeprecationType.Deprecate, 2)]
        public const string DB_NAME_OLD = "moneyfox.sqlite";

        /// <summary>
        ///     Name of the database backup
        /// </summary>
        [Deprecated("Use BACKUP_NAME instead", DeprecationType.Deprecate, 2)]
        public const string BACKUP_NAME_OLD = "backupmoneyfox.sqlite";

        /// <summary>
        ///     Name of the sqlite database
        /// </summary>
        public const string DB_NAME = "moneyfox.db";

        /// <summary>
        ///     Name of the Backup Folder
        /// </summary>
        public const string BACKUP_FOLDER_NAME = "MoneyFoxBackup";

        /// <summary>
        ///     Name of the database backup
        /// </summary>
        public const string BACKUP_NAME = "backupmoneyfox.db";

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

        public const string MSA_CLIENT_ID = "<id>";

        public const string MSA_CLIENT_SECRET = "<secret>";

        /// <summary>
        ///     Scopes for OneDrive access
        /// </summary>
        public static string[] Scopes = {"onedrive.readwrite", "wl.offline_access", "wl.signin", "onedrive.readonly"};
    }
}