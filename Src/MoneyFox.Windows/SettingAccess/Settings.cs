using System;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Platform;
using PropertyChanged;

namespace MoneyFox.Core.SettingAccess
{
    [ImplementPropertyChanged]
    public class Settings
    {
        // Settings Names
        private const string DEFAULT_ACCOUNT_KEYNAME = "DefaultAccount";
        private const string SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME = "ShowCashFlowOnMainTile";
        private const string AUTOUPLOAD_BACKUP_KEYNAME = "AutoUploadBackup";
        private const string SESSION_TIMESTAMP_KEY = "session_timestamp";
        private const string PASSWORD_REQUIRED_KEYNAME = "PasswordRequired";
        private const string DATABASE_LAST_UPDATE_KEYNAME = "DatabaseLastUpdate";

        // Default Settings
        private const int DEFAULT_ACCOUNT_KEYDEFAULT = -1;
        private const bool SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT = false;
        private const bool AUTOUPLOAD_BACKUP_KEYDEFAULT = false;
        private const string SESSION_TIMESTAMP_DEFAULT = "";
        private const bool PASSWORD_REQUIRED_KEYDEFAULT = false;
        private static DateTime DatabaseLastUpdateKeydefault { get; } = DateTime.MinValue;

        private static ILocalSettings LocalSettings => Mvx.Resolve<ILocalSettings>();
        private static IRoamingSettings RoamingSettings => Mvx.Resolve<IRoamingSettings>();

        #region Properties

        public static int DefaultAccount
        {
            get { return RoamingSettings.GetValueOrDefault(DEFAULT_ACCOUNT_KEYNAME, DEFAULT_ACCOUNT_KEYDEFAULT); }
            set { RoamingSettings.AddOrUpdateValue(DEFAULT_ACCOUNT_KEYNAME, value); }
        }

        public static bool ShowCashFlowOnMainTile
        {
            get
            {
                return RoamingSettings.GetValueOrDefault(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME,
                    SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT);
            }
            set { RoamingSettings.AddOrUpdateValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, value); }
        }

        public static bool IsBackupAutouploadEnabled
        {
            get
            {
                return RoamingSettings.GetValueOrDefault(AUTOUPLOAD_BACKUP_KEYNAME,
                    AUTOUPLOAD_BACKUP_KEYDEFAULT);
            }
            set { RoamingSettings.AddOrUpdateValue(AUTOUPLOAD_BACKUP_KEYNAME, value); }
        }

        public static string SessionTimestamp
        {
            get { return LocalSettings.GetValueOrDefault(SESSION_TIMESTAMP_KEY, SESSION_TIMESTAMP_DEFAULT); }
            set { LocalSettings.AddOrUpdateValue(SESSION_TIMESTAMP_KEY, value); }
        }

        public static bool PasswordRequired
        {
            get { return LocalSettings.GetValueOrDefault(PASSWORD_REQUIRED_KEYNAME, PASSWORD_REQUIRED_KEYDEFAULT); }
            set { LocalSettings.AddOrUpdateValue(PASSWORD_REQUIRED_KEYNAME, value); }
        }

        public static DateTime LastDatabaseUpdate
        {
            get { return LocalSettings.GetValueOrDefault(DATABASE_LAST_UPDATE_KEYNAME, DatabaseLastUpdateKeydefault); }
            set { LocalSettings.AddOrUpdateValue(DATABASE_LAST_UPDATE_KEYNAME, value); }
        }

        #endregion Properties
    }
}