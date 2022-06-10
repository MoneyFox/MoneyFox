namespace MoneyFox.Core._Pending_.Common.Facades
{

    using System;
    using System.Globalization;
    using Interfaces;

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
        private const string AutoUploadBackupKeyName = "AutoUploadBackup";
        private const bool AutoUploadBackupKeyDefault = false;

        private const string BackupLoggedInKeyName = "BackupLoggedIn";
        private const bool BackupLoggedInKeyDefault = false;

        private const string LastExecutionTimeStampSyncBackupKeyName = "LastExecutionTimeStampSyncBackup";
        private const string LastExecutionTimeStampSyncBackupKeyDefault = "";

        private const string LastExecutionTimeStampClearPaymentsKeyName = "LastExecutionTimeStampClearPayments";
        private const string LastExecutionTimeStampClearPaymentsKeyDefault = "";

        private const string LastExecutionTimeStampRecurringPaymentsKeyName = "LastExecutionTimeStampRecurringPayments";

        private const string LastExecutionTimeStampRecurringPaymentsKeyDefault = "";

        private const string DefaultCultureKeyName = "DefaultCulture";

        private const string DatabaseLastUpdateKeyName = "DatabaseLastUpdate";

        private const string IsSetupCompletedKeyName = "IsSetupCompleted";
        private const bool IsSetupCompletedKeyDefault = false;

        private const string CategorySpreadingNumberKeyName = "CategorySpreadingNumber";
        private const int CategorySpreadingNumberDefault = 6;
        private readonly string defaultCultureKeyDefault = CultureInfo.CurrentCulture.Name;

        private readonly ISettingsAdapter settingsAdapter;

        public SettingsFacade(ISettingsAdapter settingsAdapter)
        {
            this.settingsAdapter = settingsAdapter;
        }

        public DateTime LastExecutionTimeStampClearPayments
        {
            get
            {
                if (DateTime.TryParse(
                        s: settingsAdapter.GetValue(
                            key: LastExecutionTimeStampClearPaymentsKeyName,
                            defaultValue: LastExecutionTimeStampClearPaymentsKeyDefault),
                        provider: CultureInfo.InvariantCulture,
                        styles: DateTimeStyles.None,
                        result: out var outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }

            set => settingsAdapter.AddOrUpdate(key: LastExecutionTimeStampClearPaymentsKeyName, value: value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampRecurringPayments
        {
            get
            {
                if (DateTime.TryParse(
                        s: settingsAdapter.GetValue(
                            key: LastExecutionTimeStampRecurringPaymentsKeyName,
                            defaultValue: LastExecutionTimeStampRecurringPaymentsKeyDefault),
                        provider: CultureInfo.InvariantCulture,
                        styles: DateTimeStyles.None,
                        result: out var outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }

            set => settingsAdapter.AddOrUpdate(key: LastExecutionTimeStampRecurringPaymentsKeyName, value: value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public bool IsBackupAutouploadEnabled
        {
            get => settingsAdapter.GetValue(key: AutoUploadBackupKeyName, defaultValue: AutoUploadBackupKeyDefault);
            set => settingsAdapter.AddOrUpdate(key: AutoUploadBackupKeyName, value: value);
        }

        /// <inheritdoc />
        public DateTime LastDatabaseUpdate
        {
            get
            {
                var dateString = settingsAdapter.GetValue(
                    key: DatabaseLastUpdateKeyName,
                    defaultValue: DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

                return Convert.ToDateTime(value: dateString, provider: CultureInfo.InvariantCulture);
            }

            set => settingsAdapter.AddOrUpdate(key: DatabaseLastUpdateKeyName, value: value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public bool IsLoggedInToBackupService
        {
            get => settingsAdapter.GetValue(key: BackupLoggedInKeyName, defaultValue: BackupLoggedInKeyDefault);
            set => settingsAdapter.AddOrUpdate(key: BackupLoggedInKeyName, value: value);
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampSyncBackup
        {
            get
            {
                if (DateTime.TryParse(
                        s: settingsAdapter.GetValue(key: LastExecutionTimeStampSyncBackupKeyName, defaultValue: LastExecutionTimeStampSyncBackupKeyDefault),
                        provider: CultureInfo.InvariantCulture,
                        styles: DateTimeStyles.None,
                        result: out var outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }

            set => settingsAdapter.AddOrUpdate(key: LastExecutionTimeStampSyncBackupKeyName, value: value.ToString(CultureInfo.InvariantCulture));
        }

        public string DefaultCulture
        {
            get => settingsAdapter.GetValue(key: DefaultCultureKeyName, defaultValue: defaultCultureKeyDefault);
            set => settingsAdapter.AddOrUpdate(key: DefaultCultureKeyName, value: value);
        }

        /// <inheritdoc />
        public bool IsSetupCompleted
        {
            get => settingsAdapter.GetValue(key: IsSetupCompletedKeyName, defaultValue: IsSetupCompletedKeyDefault);
            set => settingsAdapter.AddOrUpdate(key: IsSetupCompletedKeyName, value: value);
        }

        /// <inheritdoc />
        public int CategorySpreadingNumber
        {
            get => settingsAdapter.GetValue(key: CategorySpreadingNumberKeyName, defaultValue: CategorySpreadingNumberDefault);
            set => settingsAdapter.AddOrUpdate(key: CategorySpreadingNumberKeyName, value: value);
        }
    }

}
