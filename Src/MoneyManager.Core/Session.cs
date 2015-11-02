using System;
using MoneyManager.DataAccess;
using Refractored.Xam.Settings.Abstractions;

namespace MoneyManager.Core
{
    public class Session
    {
        private readonly SettingDataAccess settings;
        private readonly ISettings settingStorage;

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
    }
}
