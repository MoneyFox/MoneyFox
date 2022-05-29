namespace MoneyFox.Core.Common.Facades
{

    using System;
    using System.Globalization;
    using MoneyFox.Core.Interfaces;

    /// <summary>
    ///     Provides access to the app settings.
    /// </summary>
    public interface ISettingsFacade
    {
        /// <summary>
        ///     Indicates if the backup shall be synchronized automatically.
        /// </summary>
        bool IsBackupAutouploadEnabled { get; set; }

        /// <summary>
        ///     Timestamp when the database was updated the last time.
        /// </summary>
        /// <value>The last database update.</value>
        DateTime LastDatabaseUpdate { get; set; }

        /// <summary>
        ///     Indicates if the user is logged in to the backup service.
        /// </summary>
        bool IsLoggedInToBackupService { get; set; }

        string DefaultCulture { get; set; }

        bool IsSetupCompleted { get; set; }

        int CategorySpreadingNumber { get; set; }

        DateTime LastExecutionTimeStampSyncBackup { get; set; }
    }

    public class SettingsFacade : ISettingsFacade
    {
        private const string AUTOUPLOAD_BACKUP_KEYNAME = "AutoUploadBackup";
        private const bool AUTOUPLOAD_BACKUP_KEYDEFAULT = false;

        private const string BACKUP_LOGGEDIN_KEYNAME = "BackupLoggedIn";
        private const bool BACKUP_LOGGEDIN_KEY_DEFAULT = false;

        private const string LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME = "LastExecutionTimeStampSyncBackup";
        private const string LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_DEFAULT = "";

        private const string LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME = "LastExecutionTimeStampClearPayments";
        private const string LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_DEFAULT = "";

        private const string LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME = "LastExecutionTimeStampRecurringPayments";

        private const string LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_DEFAULT = "";

        private const string DEFAULT_CULTURE_KEY_NAME = "DefaultCulture";

        private const string DATABASE_LAST_UPDATE_KEY_NAME = "DatabaseLastUpdate";

        private const string IS_SETUP_COMPLETED_KEY_NAME = "IsSetupCompleted";
        private const bool IS_SETUP_COMPLETED_KEY_DEFAULT = false;

        private const string CATEGORY_SPREADING_NUMBER_KEY_NAME = "CategorySpreadingNumber";
        private const int CATEGORY_SPREADING_NUMBER_DEFAULT = 6;
        private readonly string DEFAULT_CULTURE_KEY_DEFAULT = CultureInfo.CurrentCulture.Name;

        private readonly ISettingsAdapter settingsAdapter;

        public SettingsFacade(ISettingsAdapter settingsAdapter)
        {
            this.settingsAdapter = settingsAdapter;
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampClearPayments
        {
            get
            {
                if (DateTime.TryParse(
                        s: settingsAdapter.GetValue(
                            key: LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME,
                            defaultValue: LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_DEFAULT),
                        provider: CultureInfo.InvariantCulture,
                        styles: DateTimeStyles.None,
                        result: out var outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }

            set => settingsAdapter.AddOrUpdate(key: LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME, value: value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampRecurringPayments
        {
            get
            {
                if (DateTime.TryParse(
                        s: settingsAdapter.GetValue(
                            key: LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME,
                            defaultValue: LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_DEFAULT),
                        provider: CultureInfo.InvariantCulture,
                        styles: DateTimeStyles.None,
                        result: out var outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }

            set => settingsAdapter.AddOrUpdate(key: LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME, value: value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public bool IsBackupAutouploadEnabled
        {
            get => settingsAdapter.GetValue(key: AUTOUPLOAD_BACKUP_KEYNAME, defaultValue: AUTOUPLOAD_BACKUP_KEYDEFAULT);
            set => settingsAdapter.AddOrUpdate(key: AUTOUPLOAD_BACKUP_KEYNAME, value: value);
        }

        /// <inheritdoc />
        public DateTime LastDatabaseUpdate
        {
            get
            {
                var dateString = settingsAdapter.GetValue(
                    key: DATABASE_LAST_UPDATE_KEY_NAME,
                    defaultValue: DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

                return Convert.ToDateTime(value: dateString, provider: CultureInfo.InvariantCulture);
            }

            set => settingsAdapter.AddOrUpdate(key: DATABASE_LAST_UPDATE_KEY_NAME, value: value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public bool IsLoggedInToBackupService
        {
            get => settingsAdapter.GetValue(key: BACKUP_LOGGEDIN_KEYNAME, defaultValue: BACKUP_LOGGEDIN_KEY_DEFAULT);
            set => settingsAdapter.AddOrUpdate(key: BACKUP_LOGGEDIN_KEYNAME, value: value);
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampSyncBackup
        {
            get
            {
                if (DateTime.TryParse(
                        s: settingsAdapter.GetValue(
                            key: LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME,
                            defaultValue: LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_DEFAULT),
                        provider: CultureInfo.InvariantCulture,
                        styles: DateTimeStyles.None,
                        result: out var outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }

            set => settingsAdapter.AddOrUpdate(key: LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME, value: value.ToString(CultureInfo.InvariantCulture));
        }

        public string DefaultCulture
        {
            get => settingsAdapter.GetValue(key: DEFAULT_CULTURE_KEY_NAME, defaultValue: DEFAULT_CULTURE_KEY_DEFAULT);
            set => settingsAdapter.AddOrUpdate(key: DEFAULT_CULTURE_KEY_NAME, value: value);
        }

        /// <inheritdoc />
        public bool IsSetupCompleted
        {
            get => settingsAdapter.GetValue(key: IS_SETUP_COMPLETED_KEY_NAME, defaultValue: IS_SETUP_COMPLETED_KEY_DEFAULT);
            set => settingsAdapter.AddOrUpdate(key: IS_SETUP_COMPLETED_KEY_NAME, value: value);
        }

        /// <inheritdoc />
        public int CategorySpreadingNumber
        {
            get => settingsAdapter.GetValue(key: CATEGORY_SPREADING_NUMBER_KEY_NAME, defaultValue: CATEGORY_SPREADING_NUMBER_DEFAULT);
            set => settingsAdapter.AddOrUpdate(key: CATEGORY_SPREADING_NUMBER_KEY_NAME, value: value);
        }
    }

}
