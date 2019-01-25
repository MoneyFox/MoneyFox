using System;

namespace MoneyFox.BusinessLogic.Authentication
{
    /// <summary>
    ///     Represents a user Session.
    /// </summary>
    public class Session
    {
        /// <summary>
        ///     Amount of minutes after which the session shall expire.
        /// </summary>
        private const int SESSION_TIMEOUT = 10;

        private readonly ISettingsManager settingsManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public Session(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Validates if a session is expired.
        /// </summary>
        public bool ValidateSession()
        {
            if (!settingsManager.PasswordRequired && !settingsManager.PassportEnabled)
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