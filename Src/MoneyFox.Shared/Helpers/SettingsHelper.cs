using System;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Platform;
using PropertyChanged;

namespace MoneyFox.Shared.Helpers
{
    /// <summary>
    ///     Helps accessing the settings for this App.
    ///     NOTE: be sure that you have registered a dependency for <see cref="ISettings" /> and
    ///     <see cref="IAutobackupManager" />.
    /// </summary>
    [ImplementPropertyChanged]
    public static class SettingsHelper
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

        private const string DATABASE_LAST_UPDATE_KEYNAME = "DatabaseLastUpdate";

        /// <summary>
        ///     Constant for the Theme Setting
        ///     This is public because we have to access the setting directly in the Windows App.xaml.cs to set the theme.
        /// </summary>
        public const string DARK_THEME_SELECTED = "dark_theme_selected";

        private const bool DARK_THEME_SELECTED_KEYDEFAULT = false;
        private static DateTime DatabaseLastUpdateKeydefault { get; } = DateTime.MinValue;

        private static ISettings Settings => Mvx.Resolve<ISettings>();

        #region Properties

        public static int DefaultAccount
        {
            get { return Settings.GetValue(DEFAULT_ACCOUNT_KEYNAME, DEFAULT_ACCOUNT_KEYDEFAULT, true); }
            set { Settings.AddOrUpdateValue(DEFAULT_ACCOUNT_KEYNAME, value); }
        }

        public static bool ShowCashFlowOnMainTile
        {
            get
            {
                return Settings.GetValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME,
                    SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT);
            }
            set { Settings.AddOrUpdateValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, value); }
        }

        public static bool IsBackupAutouploadEnabled
        {
            get
            {
                return Settings.GetValue(AUTOUPLOAD_BACKUP_KEYNAME,
                    AUTOUPLOAD_BACKUP_KEYDEFAULT);
            }
            set { Settings.AddOrUpdateValue(AUTOUPLOAD_BACKUP_KEYNAME, value); }
        }

        public static string SessionTimestamp
        {
            get { return Settings.GetValue(SESSION_TIMESTAMP_KEY, SESSION_TIMESTAMP_DEFAULT); }
            set { Settings.AddOrUpdateValue(SESSION_TIMESTAMP_KEY, value); }
        }

        public static bool PasswordRequired
        {
            get { return Settings.GetValue(PASSWORD_REQUIRED_KEYNAME, PASSWORD_REQUIRED_KEYDEFAULT); }
            set { Settings.AddOrUpdateValue(PASSWORD_REQUIRED_KEYNAME, value); }
        }

        public static DateTime LastDatabaseUpdate
        {
            get { return Settings.GetValue(DATABASE_LAST_UPDATE_KEYNAME, DatabaseLastUpdateKeydefault); }
            set
            {
                Settings.AddOrUpdateValue(DATABASE_LAST_UPDATE_KEYNAME, value);
                Mvx.Resolve<IAutobackupManager>().UploadBackupIfNewwer();
            }
        }

        public static bool IsDarkThemeSelected
        {
            get { return Settings.GetValue(DARK_THEME_SELECTED, DARK_THEME_SELECTED_KEYDEFAULT); }
            set { Settings.AddOrUpdateValue(DARK_THEME_SELECTED, value); }
        }

        #endregion Properties
    }
}