using System;
using MoneyManager.Core.Helpers;
using MoneyManager.DataAccess;

namespace MoneyManager.Core.Authentication
{
    public class Session
    {
        private readonly SettingDataAccess settings;

        /// <summary>
        ///     Amount of minutes after which the session shall expire.
        /// </summary>
        private const int SESSION_TIMEOUT = 10;

        public Session(SettingDataAccess settings)
        {
            this.settings = settings;
        }

        /// <summary>
        ///     Validates if a session is expired.
        /// </summary>
        public bool ValidateSession()
        {
            if (!settings.PasswordRequired) return true;
            var entry = Settings.SessionTimestamp;


            return !string.IsNullOrEmpty(entry) && CheckIfSessionExpired();
        }

        private static bool CheckIfSessionExpired()
        {
            return (DateTime.Now - Convert.ToDateTime(Settings.SessionTimestamp)).TotalMinutes < SESSION_TIMEOUT;
        }

        /// <summary>
        ///     Adds the current time as timestamp to the local settings.
        /// </summary>
        public void AddSession()
        {
            Settings.SessionTimestamp = DateTime.Now.ToString();
        }
    }
}
