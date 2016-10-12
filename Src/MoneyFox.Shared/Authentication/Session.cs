using System;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Shared.Authentication
{
    public class Session
    {
        /// <summary>
        ///     Amount of minutes after which the session shall expire.
        /// </summary>
        private const int SESSION_TIMEOUT = 10;

        private readonly ISettingsManager settingsManager;

        public Session(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Validates if a session is expired.
        /// </summary>
        public bool ValidateSession()
        {
            if (!settingsManager.PasswordRequired)
            {
                return true;
            }

            return !string.IsNullOrEmpty(settingsManager.SessionTimestamp) && CheckIfSessionExpired();
        }

        private bool CheckIfSessionExpired()
            => (DateTime.Now - Convert.ToDateTime(settingsManager.SessionTimestamp)).TotalMinutes < SESSION_TIMEOUT;

        /// <summary>
        ///     Adds the current time as timestamp to the local settings.
        /// </summary>
        public void AddSession()
        {
            settingsManager.SessionTimestamp = DateTime.Now.ToString();
        }
    }
}