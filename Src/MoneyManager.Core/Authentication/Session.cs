using System;
using MoneyManager.Core.Helpers;

namespace MoneyManager.Core.Authentication
{
    public class Session
    {
        /// <summary>
        ///     Amount of minutes after which the session shall expire.
        /// </summary>
        private const int SESSION_TIMEOUT = 10;

        /// <summary>
        ///     Validates if a session is expired.
        /// </summary>
        public bool ValidateSession()
        {
            if (!Settings.PasswordRequired) return true;

            return !string.IsNullOrEmpty(Settings.SessionTimestamp) && CheckIfSessionExpired();
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