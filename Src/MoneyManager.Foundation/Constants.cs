namespace MoneyManager.Foundation
{
    /// <summary>
    ///     String Constants for usage in the app
    /// </summary>
    public class Constants
    {
        /// <summary>
        ///     Name of the sqlite database
        /// </summary>
        public const string DB_NAME = "moneyfox.sqlite";

        /// <summary>
        ///     Name of the Backup Folder
        /// </summary>
        public const string BACKUP_FOLDER_NAME = "MoneyFoxBackup";

        /// <summary>
        ///     Name of the database backup
        /// </summary>
        public const string BACKUP_NAME = "backupmoneyfox.sqlite";

        /// <summary>
        ///     ID string for income shortcuts
        /// </summary>
        public const string INCOME_TILE_ID = "Income";

        /// <summary>
        ///     ID string for spending shortcuts
        /// </summary>
        public const string SPENDING_TILE_ID = "Spending";

        /// <summary>
        ///     ID string for transfer shortcuts
        /// </summary>
        public const string TRANSFER_TILE_ID = "Transfer";
    }
}