using MoneyFox.Application.Common.Adapters;
using System;
using System.Globalization;

namespace MoneyFox.Application.Common.Facades
{
    /// <summary>
    ///     Provides access to the app settings.
    /// </summary>
    public interface ISettingsFacade
    {
        /// <summary>
        ///     Indicates if the backup shall be synced automatically.
        /// </summary>
        bool IsBackupAutouploadEnabled { get; set; }

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
        ///     Returns the time stamp when the last job to create recurring payments was executed.
        /// </summary>
        DateTime LastExecutionTimeStampRecurringPayments { get; set; }
    }

    public class SettingsFacade : ISettingsFacade
    {
        private const string AUTOUPLOAD_BACKUP_KEYNAME = "AutoUploadBackup";
        private const bool AUTOUPLOAD_BACKUP_KEYDEFAULT = false;

        private const string BACKUP_LOGGEDIN_KEYNAME = "BackupLoggedIn";
        private const bool BACKUP_LOGGEDIN_KEY_DEFAULT = false;

        private const string BACKUP_SYNC_RECURRENCE_KEYNAME = "BackupSyncRecurrence";
        private const int BACKUP_SYNC_RECURRENCE_KEY_DEFAULT = 3;

        private const string THEME_KEYNAME = "Theme";
        private const int THEME_KEYDEFAULT = (int)AppTheme.Light;

        private const string LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME = "LastExecutionTimeStampSyncBackup";
        private const string LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_DEFAULT = "";

        private const string LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME = "LastExecutionTimeStampClearPayments";
        private const string LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_DEFAULT = "";

        private const string LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME = "LastExecutionTimeStampRecurringPayments";
        private const string LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_DEFAULT = "";

        private const string DATABASE_LAST_UPDATE_KEYNAME = "DatabaseLastUpdate";

        private readonly ISettingsAdapter settingsAdapter;

        public SettingsFacade(ISettingsAdapter settingsAdapter)
        {
            this.settingsAdapter = settingsAdapter;
        }

        /// <inheritdoc />
        public bool IsBackupAutouploadEnabled
        {
            get => settingsAdapter.GetValue(AUTOUPLOAD_BACKUP_KEYNAME, AUTOUPLOAD_BACKUP_KEYDEFAULT);
            set => settingsAdapter.AddOrUpdate(AUTOUPLOAD_BACKUP_KEYNAME, value);
        }

        /// <inheritdoc />
        public DateTime LastDatabaseUpdate
        {
            get
            {
                string dateString = settingsAdapter.GetValue(DATABASE_LAST_UPDATE_KEYNAME,
                                                             DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

                return Convert.ToDateTime(dateString, CultureInfo.InvariantCulture);
            }
            set => settingsAdapter.AddOrUpdate(DATABASE_LAST_UPDATE_KEYNAME,
                                               value.ToString(CultureInfo.InvariantCulture));
        }

        public AppTheme Theme
        {
            get
            {
                int themeInt = settingsAdapter.GetValue(THEME_KEYNAME, THEME_KEYDEFAULT);

                return (AppTheme) Enum.ToObject(typeof(AppTheme), themeInt);
            }
            set => settingsAdapter.AddOrUpdate(THEME_KEYNAME, (int) value);
        }

        /// <inheritdoc />
        public bool IsLoggedInToBackupService
        {
            get => settingsAdapter.GetValue(BACKUP_LOGGEDIN_KEYNAME, BACKUP_LOGGEDIN_KEY_DEFAULT);
            set => settingsAdapter.AddOrUpdate(BACKUP_LOGGEDIN_KEYNAME, value);
        }

        /// <inheritdoc />
        public int BackupSyncRecurrence
        {
            get => settingsAdapter.GetValue(BACKUP_SYNC_RECURRENCE_KEYNAME, BACKUP_SYNC_RECURRENCE_KEY_DEFAULT);
            set => settingsAdapter.AddOrUpdate(BACKUP_SYNC_RECURRENCE_KEYNAME, value);
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampSyncBackup
        {
            get
            {
                if (DateTime.TryParse(settingsAdapter.GetValue(LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME,
                                                               LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_DEFAULT),
                                      CultureInfo.InvariantCulture,
                                      DateTimeStyles.None,
                                      out DateTime outValue))
                    return outValue;

                return DateTime.MinValue;
            }
            set => settingsAdapter.AddOrUpdate(LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampClearPayments
        {
            get
            {
                if (DateTime.TryParse(settingsAdapter.GetValue(LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME,
                                                               LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_DEFAULT),
                                      CultureInfo.InvariantCulture,
                                      DateTimeStyles.None,
                                      out DateTime outValue))
                    return outValue;

                return DateTime.MinValue;
            }
            set => settingsAdapter.AddOrUpdate(LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampRecurringPayments
        {
            get
            {
                if (DateTime.TryParse(settingsAdapter.GetValue(LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME,
                                                               LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_DEFAULT),
                                      CultureInfo.InvariantCulture,
                                      DateTimeStyles.None,
                                      out DateTime outValue))
                    return outValue;

                return DateTime.MinValue;
            }
            set => settingsAdapter.AddOrUpdate(LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME, value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
