using System;
using MoneyManager.Foundation.Interfaces;
using MvvmCross.Platform;

namespace MoneyManager.Core.Helpers
{
    /// <summary>
    ///     This is the Settings static class that can be used in your Core solution or in any
    ///     of your client applications. All settings are laid out the same exact way with getters
    ///     and setters.
    /// </summary>
    public static class Settings
    {
        private const string SESSION_TIMESTAMP_KEY = "session_timestamp";
        private const string PASSWORD_REQUIRED_KEYNAME = "PasswordRequired";
        private const string DATABASE_LAST_UPDATE_KEYNAME = "DatabaseLastUpdate";

        private const string SESSION_TIMESTAMP_DEFAULT = "";
        private const bool PASSWORD_REQUIRED_KEYDEFAULT = false;
        private static DateTime DatabaseLastUpdateKeydefault { get; } = DateTime.MinValue;

        private static ILocalSettings AppSettings => Mvx.Resolve<ILocalSettings>();

        public static string SessionTimestamp
        {
            get { return AppSettings.GetValueOrDefault(SESSION_TIMESTAMP_KEY, SESSION_TIMESTAMP_DEFAULT); }
            set { AppSettings.AddOrUpdateValue(SESSION_TIMESTAMP_KEY, value); }
        }

        public static bool PasswordRequired
        {
            get { return AppSettings.GetValueOrDefault(PASSWORD_REQUIRED_KEYNAME, PASSWORD_REQUIRED_KEYDEFAULT); }
            set { AppSettings.AddOrUpdateValue(PASSWORD_REQUIRED_KEYNAME, value); }
        }

        public static DateTime LastDatabaseUpdate
        {
            get { return AppSettings.GetValueOrDefault(DATABASE_LAST_UPDATE_KEYNAME, DatabaseLastUpdateKeydefault); }
            set { AppSettings.AddOrUpdateValue(DATABASE_LAST_UPDATE_KEYNAME, value); }
        }
    }
}