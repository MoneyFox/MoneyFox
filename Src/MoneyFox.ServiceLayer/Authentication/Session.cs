using System;
using MoneyFox.ServiceLayer.Facades;

namespace MoneyFox.ServiceLayer.Authentication
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

        private readonly ISettingsFacade settingsFacade;

        /// <summary>
        ///     Constructor
        /// </summary>
        public Session(ISettingsFacade settingsManager)
        {
            this.settingsFacade = settingsManager;
        }

        /// <summary>
        ///     Validates if a session is expired.
        /// </summary>
        public bool ValidateSession()
        {
            if (!settingsFacade.PasswordRequired && !settingsFacade.PassportEnabled)
            {
                return true;
            }

            return !string.IsNullOrEmpty(settingsFacade.SessionTimestamp) && CheckIfSessionExpired();
        }

        private bool CheckIfSessionExpired()
            => (DateTime.Now - Convert.ToDateTime(settingsFacade.SessionTimestamp)).TotalMinutes < SESSION_TIMEOUT;

        /// <summary>
        ///     Adds the current time as timestamp to the local settings.
        /// </summary>
        public void AddSession()
        {
            settingsFacade.SessionTimestamp = DateTime.Now.ToString();
        }
    }
}