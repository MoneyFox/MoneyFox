using System;

namespace MoneyFox.Foundation.Interfaces
{
    /// <summary>
    ///     Provides access to the app settings.
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        ///     Indicates if the cash flow shall be displayed on the main tile or not.
        /// </summary>
        bool ShowCashFlowOnMainTile { get; set; }

        /// <summary>
        ///     Indicates if the backup shall be synced automatically.
        /// </summary>
        bool IsBackupAutouploadEnabled { get; set; }

        /// <summary>
        ///     Timestamp of the session
        /// </summary>
        /// <value>The session timestamp.</value>
        string SessionTimestamp { get; set; }

        /// <summary>
        ///     Indicates if a password is required to login.
        /// </summary>
        bool PasswordRequired { get; set; }

        /// <summary>
        ///     Indicates if the passport login is activated.
        /// </summary>
        bool PassportEnabled { get; set; }

        /// <summary>
        ///     Timestamp when the database was updated the last time.
        /// </summary>
        /// <value>The last database update.</value>
        DateTime LastDatabaseUpdate { get; set; }
        
        /// <summary>
        ///     Currently selected theme.
        /// </summary>
        AppTheme Theme { get; set; }

        /// <summary>
        ///     Indicates if the user is logged in to the backup service.
        /// </summary>
        bool IsLoggedInToBackupService { get; set; }

        /// <summary>
        ///     Recurrence to sync the backup.
        /// </summary>
        /// <value>The backup sync recurrence in hours..</value>
        int BackupSyncRecurrence { get; set; }

        /// <summary>
        ///     Returns the timestamp when the last sync backup job was executed.
        /// </summary>
        DateTime LastExecutionTimeStampSyncBackup { get; set; }

        /// <summary>
        ///     Returns the timestamp when the last clear payment job was executed.
        /// </summary>
        DateTime LastExecutionTimeStampClearPayments { get; set; }

        /// <summary>
        ///     Returns the timestamp when the last job to create recurring payments was executed.
        /// </summary>
        DateTime LastExecutionTimeStampRecurringPayments { get; set; }
    }
}