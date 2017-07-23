﻿using System;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.Manager
{
    
    public class SettingsManager : ISettingsManager
    {
        private const string DEFAULT_ACCOUNT_KEYNAME = "DefaultAccount";
        private const int DEFAULT_ACCOUNT_KEYDEFAULT = -1;

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
        private const bool BACKUP_LOGGEDIN_KEYDEFAULT = false;

        private const string BACKUP_SYNC_RECURRENCE_KEYNAME = "BackupSyncRecurrence";
        private const int BACKUP_SYNC_RECURRENCE_KEYDEFAULT = 3;

        private const string DATABASE_LAST_UPDATE_KEYNAME = "DatabaseLastUpdate";

        /// <summary>
        ///     Constant for the Theme Setting
        ///     This is public because we have to access the setting directly in the Windows App.xaml.cs to set the theme.
        /// </summary>
        public const string DARK_THEME_SELECTED_KEYNAME = "dark_theme_selected";
        private const bool DARK_THEME_SELECTED_KEYDEFAULT = false;

        public const string USE_SYSTEM_THEME_KEYNAME = "use_system_theme";
        private const bool USE_SYSTEM_THEME_KEYDEFAULT = true;

        private readonly ISettings settings;

        public SettingsManager(ISettings settings)
        {
            this.settings = settings;
        }

        #region Properties

        /// <inheritdoc />
        public bool ShowCashFlowOnMainTile
        {
            get
            {
                return settings.GetValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME,
                    SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT);
            }
            set { settings.AddOrUpdateValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, value); }
        }

        /// <inheritdoc />
        public bool IsBackupAutouploadEnabled
        {
            get
            {
                return settings.GetValue(AUTOUPLOAD_BACKUP_KEYNAME,
                    AUTOUPLOAD_BACKUP_KEYDEFAULT);
            }
            set { settings.AddOrUpdateValue(AUTOUPLOAD_BACKUP_KEYNAME, value); }
        }

        /// <inheritdoc />
        public string SessionTimestamp
        {
            get { return settings.GetValue(SESSION_TIMESTAMP_KEY, SESSION_TIMESTAMP_DEFAULT); }
            set { settings.AddOrUpdateValue(SESSION_TIMESTAMP_KEY, value); }
        }

        /// <inheritdoc />
        public bool PasswordRequired
        {
            get { return settings.GetValue(PASSWORD_REQUIRED_KEYNAME, PASSWORD_REQUIRED_KEYDEFAULT); }
            set { settings.AddOrUpdateValue(PASSWORD_REQUIRED_KEYNAME, value); }
        }

        /// <inheritdoc />
        public bool PassportEnabled {
            get { return settings.GetValue(PASSPORT_REQUIRED_KEYNAME, PASSPORT_REQUIRED_KEYDEFAULT); }
            set { settings.AddOrUpdateValue(PASSPORT_REQUIRED_KEYNAME, value); }
        }

        /// <inheritdoc />
        public DateTime LastDatabaseUpdate
        {
            get { return settings.GetValue(DATABASE_LAST_UPDATE_KEYNAME, DateTime.MinValue); }
            set { settings.AddOrUpdateValue(DATABASE_LAST_UPDATE_KEYNAME, value); }
        }

        /// <inheritdoc />
        public bool IsDarkThemeSelected
        {
            get { return settings.GetValue(DARK_THEME_SELECTED_KEYNAME, DARK_THEME_SELECTED_KEYDEFAULT); }
            set { settings.AddOrUpdateValue(DARK_THEME_SELECTED_KEYNAME, value); }
        }

        /// <inheritdoc />
        public bool UseSystemTheme
        {
            get { return settings.GetValue(USE_SYSTEM_THEME_KEYNAME, USE_SYSTEM_THEME_KEYDEFAULT); }
            set { settings.AddOrUpdateValue(USE_SYSTEM_THEME_KEYNAME, value); }
        }

        /// <inheritdoc />
        public bool IsLoggedInToBackupService
        {
            get { return settings.GetValue(BACKUP_LOGGEDIN_KEYNAME, BACKUP_LOGGEDIN_KEYDEFAULT); }
            set { settings.AddOrUpdateValue(BACKUP_LOGGEDIN_KEYNAME, value); }
        }

        /// <inheritdoc />
        public int BackupSyncRecurrence
        {
            get { return settings.GetValue(BACKUP_SYNC_RECURRENCE_KEYNAME, BACKUP_SYNC_RECURRENCE_KEYDEFAULT); }
            set { settings.AddOrUpdateValue(BACKUP_SYNC_RECURRENCE_KEYNAME, value); }
        }

        #endregion Properties
    }
}