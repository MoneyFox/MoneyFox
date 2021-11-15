﻿using MoneyFox.Application.Common.Adapters;
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
        ///     Indicates if the backup shall be synchronized automatically.
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

        private const string THEME_KEYNAME = "Theme";
        private const int THEME_KEYDEFAULT = (int)AppTheme.Light;

        private const string LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME = "LastExecutionTimeStampSyncBackup";
        private const string LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_DEFAULT = "";

        private const string LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME = "LastExecutionTimeStampClearPayments";
        private const string LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_DEFAULT = "";

        private const string LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME =
            "LastExecutionTimeStampRecurringPayments";

        private const string LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_DEFAULT = "";

        private const string DEFAULT_CULTURE_KEYNAME = "DefaultCulture";
        private readonly string DEFAULT_CULTURE_KEYDEFAULT = CultureInfo.CurrentCulture.Name;

        private const string DATABASE_LAST_UPDATE_KEYNAME = "DatabaseLastUpdate";

        private const string IS_SETUP_COMPLETED_KEYNAME = "IsSetupCompleted";
        private const bool IS_SETUP_COMPLETED__KEY_DEFAULT = false;

        private const string CATEGORY_SPREADING_NUMBER_KEYNAME = "CategorySpreadingNumber";
        private const int CATEGORY_SPREADING_NUMBER_DEFAULT = 6;

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
                string dateString = settingsAdapter.GetValue(
                    DATABASE_LAST_UPDATE_KEYNAME,
                    DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

                return Convert.ToDateTime(dateString, CultureInfo.InvariantCulture);
            }
            set
                => settingsAdapter.AddOrUpdate(
                    DATABASE_LAST_UPDATE_KEYNAME,
                    value.ToString(CultureInfo.InvariantCulture));
        }

        public AppTheme Theme
        {
            get
            {
                int themeInt = settingsAdapter.GetValue(THEME_KEYNAME, THEME_KEYDEFAULT);

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
        public DateTime LastExecutionTimeStampSyncBackup
        {
            get
            {
                if(DateTime.TryParse(
                       settingsAdapter.GetValue(
                           LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME,
                           LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_DEFAULT),
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }
            set
                => settingsAdapter.AddOrUpdate(
                    LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME,
                    value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampClearPayments
        {
            get
            {
                if(DateTime.TryParse(
                       settingsAdapter.GetValue(
                           LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME,
                           LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_DEFAULT),
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }
            set
                => settingsAdapter.AddOrUpdate(
                    LAST_EXECUTION_TIME_STAMP_CLEAR_PAYMENTS_KEY_NAME,
                    value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc />
        public DateTime LastExecutionTimeStampRecurringPayments
        {
            get
            {
                if(DateTime.TryParse(
                       settingsAdapter.GetValue(
                           LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME,
                           LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_DEFAULT),
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime outValue))
                {
                    return outValue;
                }

                return DateTime.MinValue;
            }
            set
                => settingsAdapter.AddOrUpdate(
                    LAST_EXECUTION_TIME_STAMP_RECURRING_PAYMENTS_KEY_NAME,
                    value.ToString(CultureInfo.InvariantCulture));
        }

        public string DefaultCulture
        {
            get => settingsAdapter.GetValue(DEFAULT_CULTURE_KEYNAME, DEFAULT_CULTURE_KEYDEFAULT);
            set => settingsAdapter.AddOrUpdate(DEFAULT_CULTURE_KEYNAME, value);
        }

        /// <inheritdoc />
        public bool IsSetupCompleted
        {
            get => settingsAdapter.GetValue(IS_SETUP_COMPLETED_KEYNAME, IS_SETUP_COMPLETED__KEY_DEFAULT);
            set => settingsAdapter.AddOrUpdate(IS_SETUP_COMPLETED_KEYNAME, value);
        }

        /// <inheritdoc />
        public int CategorySpreadingNumber
        {
            get => settingsAdapter.GetValue(CATEGORY_SPREADING_NUMBER_KEYNAME, CATEGORY_SPREADING_NUMBER_DEFAULT);
            set => settingsAdapter.AddOrUpdate(CATEGORY_SPREADING_NUMBER_KEYNAME, value);
        }
    }
}