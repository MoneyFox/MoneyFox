namespace MoneyFox.Application.Common.Constants
{
    /// <summary>
    /// Contains constant values regarding the database
    /// </summary>
    public static class DatabaseConstants
    {
        /// <summary>
        /// Name of the Backup Folder
        /// </summary>
        public static string ARCHIVE_FOLDER_NAME => "Archive";

        /// <summary>
        /// Name of the database backup
        /// </summary>
        public static string BACKUP_NAME => "backupmoneyfox3.db";

        /// <summary>
        /// Name of the database backup archive
        /// </summary>
        public static string BACKUP_ARCHIVE_NAME => "backupmoneyfox3_{0}.db";
    }
}
