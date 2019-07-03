#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace MoneyFox.Application.Constants
{
    /// <summary>
    ///     Contains constant values regarding the database
    /// </summary>
    public static class DatabaseConstants
    {
        /// <summary>
        ///     Name of the SQLite database
        /// </summary>
        public const string DB_NAME = "moneyfox3.db";

        /// <summary>
        ///     Name of the Backup Folder
        /// </summary>
        public const string BACKUP_FOLDER_NAME = "MoneyFoxBackup";

        /// <summary>
        ///     Name of the Backup Folder
        /// </summary>
        public const string ARCHIVE_FOLDER_NAME = "Archive";

        /// <summary>
        ///     Name of the database backup
        /// </summary>
        public const string BACKUP_NAME = "backupmoneyfox3.db";

        /// <summary>
        ///     Name of the database backup archive
        /// </summary>
        public const string BACKUP_ARCHIVE_NAME = "backupmoneyfox3_{0}.db";
    }
}