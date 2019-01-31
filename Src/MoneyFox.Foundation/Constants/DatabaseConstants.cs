#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace MoneyFox.Foundation.Constants
{
    /// <summary>
    ///     Contains constant values regarding the database
    /// </summary>
    public sealed class DatabaseConstants
    {
        /// <summary>
        ///     Name of the SQLite database with the old schema
        /// </summary>
        public const string DB_NAME_OLD = "moneyfox2.db";

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
        ///     Name of the database backup with the old schema
        /// </summary>
        public const string BACKUP_NAME_OLD = "backupmoneyfox2.db";

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