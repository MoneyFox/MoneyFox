﻿using System;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
 using MoneyFox.Foundation;
 using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.Manager
{
    
    public class SettingsManager : ISettingsManager
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
        private const bool BACKUP_LOGGEDIN_KEYDEFAULT = false;

        private const string BACKUP_SYNC_RECURRENCE_KEYNAME = "BackupSyncRecurrence";
        private const int BACKUP_SYNC_RECURRENCE_KEYDEFAULT = 3;

        public const string THEME_KEYNAME = "Theme";
        private const AppTheme THEME_KEYDEFAULT = AppTheme.Dark;

        private const string DATABASE_LAST_UPDATE_KEYNAME = "DatabaseLastUpdate";
        
        private readonly ISettings settings;

        public SettingsManager(ISettings settings)
        {
            this.settings = settings;
        }

        #region Properties

        /// <inheritdoc />
        public bool ShowCashFlowOnMainTile
        {
            get => settings.GetValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME,SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT);
            set => settings.AddOrUpdateValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, value);
        }

        /// <inheritdoc />
        public bool IsBackupAutouploadEnabled
        {
            get => settings.GetValue(AUTOUPLOAD_BACKUP_KEYNAME, AUTOUPLOAD_BACKUP_KEYDEFAULT);
            set => settings.AddOrUpdateValue(AUTOUPLOAD_BACKUP_KEYNAME, value);
        }

        /// <inheritdoc />
        public string SessionTimestamp
        {
            get => settings.GetValue(SESSION_TIMESTAMP_KEY, SESSION_TIMESTAMP_DEFAULT);
            set => settings.AddOrUpdateValue(SESSION_TIMESTAMP_KEY, value);
        }

        /// <inheritdoc />
        public bool PasswordRequired
        {
            get => settings.GetValue(PASSWORD_REQUIRED_KEYNAME, PASSWORD_REQUIRED_KEYDEFAULT);
            set => settings.AddOrUpdateValue(PASSWORD_REQUIRED_KEYNAME, value);
        }

        /// <inheritdoc />
        public bool PassportEnabled {
            get => settings.GetValue(PASSPORT_REQUIRED_KEYNAME, PASSPORT_REQUIRED_KEYDEFAULT);
            set => settings.AddOrUpdateValue(PASSPORT_REQUIRED_KEYNAME, value);
        }

        /// <inheritdoc />
        public DateTime LastDatabaseUpdate
        {
            get => settings.GetValue(DATABASE_LAST_UPDATE_KEYNAME, DateTime.MinValue);
            set => settings.AddOrUpdateValue(DATABASE_LAST_UPDATE_KEYNAME, value);
        }

        public AppTheme Theme
        {
            get => settings.GetValue(THEME_KEYNAME, THEME_KEYDEFAULT);
            set => settings.AddOrUpdateValue(THEME_KEYNAME, value);
        }

        /// <inheritdoc />
        public bool IsLoggedInToBackupService
        {
            get => settings.GetValue(BACKUP_LOGGEDIN_KEYNAME, BACKUP_LOGGEDIN_KEYDEFAULT);
            set => settings.AddOrUpdateValue(BACKUP_LOGGEDIN_KEYNAME, value);
        }

        /// <inheritdoc />
        public int BackupSyncRecurrence
        {
            get => settings.GetValue(BACKUP_SYNC_RECURRENCE_KEYNAME, BACKUP_SYNC_RECURRENCE_KEYDEFAULT);
            set => settings.AddOrUpdateValue(BACKUP_SYNC_RECURRENCE_KEYNAME, value);
        }

        #endregion Properties
    }
}