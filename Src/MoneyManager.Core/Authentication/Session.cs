using System;
using MoneyManager.DataAccess;
using Refractored.Xam.Settings.Abstractions;

namespace MoneyManager.Core.Authentication
{
    public class Session
    {
        private readonly SettingDataAccess settings;
        private readonly ISettings settingStorage;

        /// <summary>
        ///     Amount of minutes after which the session shall expire.
        /// </summary>
        private const int SESSION_TIMEOUT = 10;
        private const string SESSION_KEY = "session_timestamp";

        public Session(SettingDataAccess settings, ISettings settingStorage)
        {
            this.settings = settings;
            this.settingStorage = settingStorage;
        }

        /// <summary>
        ///     Validates if a session is expired.
        /// </summary>
        public bool ValidateSession()
        {
            if (!settings.PasswordRequired) return true;

            var entry = settingStorage.GetValueOrDefault<string>(SESSION_KEY);
            if (string.IsNullOrEmpty(entry)) return false;

            return checkIfSessionExpired(entry);
        }

        private static bool checkIfSessionExpired(string entry)
        {
            return (DateTime.Now - Convert.ToDateTime(entry)).TotalMinutes < SESSION_TIMEOUT;
        }

        /// <summary>
        ///     Adds the current time as timestamp to the local settings.
        /// </summary>
        public void AddSession()
        {
            settingStorage.AddOrUpdateValue(SESSION_KEY, DateTime.Now);
        }
    }
}
