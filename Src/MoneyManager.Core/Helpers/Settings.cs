using Cirrious.CrossCore;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Core.Helpers
{
    /// <summary>
    ///     This is the Settings static class that can be used in your Core solution or in any
    ///     of your client applications. All settings are laid out the same exact way with getters
    ///     and setters.
    /// </summary>
    public static class Settings
    {
        private const string SETTINGS_KEY = "session_timestamp";
        private const string SETTINGS_DEFAULT = "";

        private static ILocalSettings AppSettings => Mvx.Resolve<ILocalSettings>();

        public static string SessionTimestamp
        {
            get { return AppSettings.GetValueOrDefault(SETTINGS_KEY, SETTINGS_DEFAULT); }
            set { AppSettings.AddOrUpdateValue(SETTINGS_KEY, value); }
        }
    }
}