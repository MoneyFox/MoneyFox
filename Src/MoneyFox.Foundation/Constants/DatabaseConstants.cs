namespace MoneyFox.Foundation.Constants
{
    /// <summary>
    ///     Contains constant values regarding the database
    /// </summary>
    public class DatabaseConstants
    {
        /// <summary>
        ///     Name of the sqlite database
        ///     DEPREDCATED: Use DB_NAME instead
        /// </summary>
        public const string DB_NAME_OLD = "moneyfox.db";

        /// <summary>
        ///     Name of the database backup
        ///     DEPREDCATED: Use BACKUP_NAME instead
        /// </summary>
        public const string BACKUP_NAME_OLD = "backupmoneyfox.db";

        /// <summary>
        ///     Name of the sqlite database
        /// </summary>
        public const string DB_NAME = "moneyfox2.db";

        /// <summary>
        ///     Name of the Backup Folder
        /// </summary>
        public const string BACKUP_FOLDER_NAME = "MoneyFoxBackup";

        /// <summary>
        ///     Name of the database backup
        /// </summary>
        public const string BACKUP_NAME = "backupmoneyfox2.db";
    }
}