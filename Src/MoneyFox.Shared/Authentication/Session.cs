using System;
using MoneyFox.Shared.Helpers;

namespace MoneyFox.Shared.Authentication
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
            if (!SettingsHelper.PasswordRequired)
            {
                return true;
            }

            return !string.IsNullOrEmpty(SettingsHelper.SessionTimestamp) && CheckIfSessionExpired();
        }

        private static bool CheckIfSessionExpired()
            => (DateTime.Now - Convert.ToDateTime(SettingsHelper.SessionTimestamp)).TotalMinutes < SESSION_TIMEOUT;

        /// <summary>
        ///     Adds the current time as timestamp to the local settings.
        /// </summary>
        public void AddSession()
        {
            SettingsHelper.SessionTimestamp = DateTime.Now.ToString();
        }
    }
}