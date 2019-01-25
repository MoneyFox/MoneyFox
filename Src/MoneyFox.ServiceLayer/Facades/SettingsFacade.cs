using System;
using System.Globalization;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Foundation;

namespace MoneyFox.ServiceLayer.Facades
{    
    /// <summary>
     ///     Provides access to the app settings.
     /// </summary>
    public interface ISettingsFacade
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

    public class SettingsFacade : ISettingsFacade
    {
        private const string SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME = "ShowCashFlowOnMainTile";
        private const bool SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT = true;

        private const string AUTOUPLOAD_BACKUP_KEYNAME = "AutoUploadBackup";
        private const bool AUTOUPLOAD_BACKUP_KEYDEFAULT = false;

        private const string SESSION_TIMESTAMP_KEY = "session_timestamp";
        private const string SESSION_TIMESTAMP_DEFAULT = "";

        private const string PASSWORD_REQUIRED_KEYNAME = "PasswordRequired";
        private const bool PASSWORD_REQUIRED_KEYDEFAULT = false;

        private const string PASSPORT_REQUIRED_KEYNAME = "PassportRequired";
        private const bool PASSPORT_REQUIRED_KEYDEFAULT = false;

        private const string BACKUP_LOGGEDIN_KEYNAME = "BackupLoggedIn";
        private const bool BACKUP_LOGGEDIN_KEY_DEFAULT = false;

        private const string BACKUP_SYNC_RECURRENCE_KEYNAME = "BackupSyncRecurrence";
        private const int BACKUP_SYNC_RECURRENCE_KEY_DEFAULT = 3;

        public const string THEME_KEYNAME = "Theme";
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
        public bool ShowCashFlowOnMainTile
        {
            get =>
                settingsAdapter.GetValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT);
            set => settingsAdapter.AddOrUpdate(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, value);
        }

        /// <inheritdoc />
        public bool IsBackupAutouploadEnabled
        {
            get => settingsAdapter.GetValue(AUTOUPLOAD_BACKUP_KEYNAME, AUTOUPLOAD_BACKUP_KEYDEFAULT);
            set => settingsAdapter.AddOrUpdate(AUTOUPLOAD_BACKUP_KEYNAME, value);
        }

        /// <inheritdoc />
        public string SessionTimestamp
        {
            get => settingsAdapter.GetValue(SESSION_TIMESTAMP_KEY, SESSION_TIMESTAMP_DEFAULT);
            set => settingsAdapter.AddOrUpdate(SESSION_TIMESTAMP_KEY, value);
        }

        /// <inheritdoc />
        public bool PasswordRequired
        {
            get => settingsAdapter.GetValue(PASSWORD_REQUIRED_KEYNAME, PASSWORD_REQUIRED_KEYDEFAULT);
            set => settingsAdapter.AddOrUpdate(PASSWORD_REQUIRED_KEYNAME, value);
        }

        /// <inheritdoc />
        public bool PassportEnabled
        {
            get => settingsAdapter.GetValue(PASSPORT_REQUIRED_KEYNAME, PASSPORT_REQUIRED_KEYDEFAULT);
            set => settingsAdapter.AddOrUpdate(PASSPORT_REQUIRED_KEYNAME, value);
        }

        /// <inheritdoc />
        public DateTime LastDatabaseUpdate
        {
            get
            {
                var dateString = settingsAdapter.GetValue(DATABASE_LAST_UPDATE_KEYNAME,
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
                var themeInt = settingsAdapter.GetValue(THEME_KEYNAME, THEME_KEYDEFAULT);
                return (AppTheme)Enum.ToObject(typeof(AppTheme), themeInt);
            }
            set => settingsAdapter.AddOrUpdate(THEME_KEYNAME, (int)value);
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
                DateTime outValue;
                if (DateTime.TryParse(settingsAdapter.GetValue(LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME,
                                                               LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_DEFAULT),
                                                               CultureInfo.InvariantCulture,
                                                               DateTimeStyles.None,
                                                               out outValue))
                {
                    return outValue;
                }
                return DateTime.MinValue;
            }
            set => settingsAdapter.AddOrUpdate(LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampClearPayments
        {
            get
            {
                DateTime outValue;
                if (DateTime.TryParse(settingsAdapter.GetValue(LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME,
                                                               LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_DEFAULT),
                                                               CultureInfo.InvariantCulture,
                                                               DateTimeStyles.None,
                                                               out outValue))
                {
                    return outValue;
                }
                return DateTime.MinValue;
            }
            set => settingsAdapter.AddOrUpdate(LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampRecurringPayments
        {
            get
            {
                DateTime outValue;
                if (DateTime.TryParse(settingsAdapter.GetValue(LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME,
                                                               LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_DEFAULT),
                                                               CultureInfo.InvariantCulture,
                                                               DateTimeStyles.None,
                                                               out outValue))
                {
                    return outValue;
                }
                return DateTime.MinValue;
            }
            set => settingsAdapter.AddOrUpdate(LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME, value.ToString(CultureInfo.InvariantCulture));
        }
    }
}