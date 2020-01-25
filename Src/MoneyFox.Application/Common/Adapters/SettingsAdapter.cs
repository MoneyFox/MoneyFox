using System;
using NLog;
using Xamarin.Essentials;

namespace MoneyFox.Application.Common.Adapters
{
    public interface ISettingsAdapter
    {
        /// <summary>
        ///     Selects bool value of the settings with the passed key.
        /// </summary>
        /// <param name="key">Settings Key.</param>
        /// <param name="defaultValue">Default value in case nothing is found.</param>
        /// <returns>value</returns>
        bool GetValue(string key, bool defaultValue);

        /// <summary>
        ///     Selects string value of the settings with the passed key.
        /// </summary>
        /// <param name="key">Settings Key.</param>
        /// <param name="defaultValue">Default value in case nothing is found.</param>
        /// <returns>value</returns>
        string GetValue(string key, string defaultValue);

        /// <summary>
        ///     Selects int value of the settings with the passed key.
        /// </summary>
        /// <param name="key">Settings Key.</param>
        /// <param name="defaultValue">Default value in case nothing is found.</param>
        /// <returns>value</returns>
        int GetValue(string key, int defaultValue);

        /// <summary>
        ///     Adds a setting with a bool value or updates it if the key already exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void AddOrUpdate(string key, bool value);

        /// <summary>
        ///     Adds a setting with a string value or updates it if the key already exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void AddOrUpdate(string key, string value);

        /// <summary>
        ///     Adds a setting with a int value or updates it if the key already exists.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void AddOrUpdate(string key, int value);

        /// <summary>
        ///     Removes the setting with the passed key.
        /// </summary>
        /// <param name="key">Settings key.</param>
        void Remove(string key);
    }

    public class SettingsAdapter : ISettingsAdapter
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public bool GetValue(string key, bool defaultValue)
        {
            try
            {
                return Preferences.Get(key, defaultValue);
            }
            catch (InvalidCastException)
            {
                logManager.Error($"Value {key} couldn't be parsed to bool.");
                Preferences.Set(key, defaultValue);
                return defaultValue;
            }
        }

        public string GetValue(string key, string defaultValue)
        {
            try
            {
                return Preferences.Get(key, defaultValue);
            }
            catch (InvalidCastException)
            {
                logManager.Error($"Value {key} couldn't be parsed to string.");

                Preferences.Set(key, defaultValue);
                return defaultValue;
            }
        }

        public int GetValue(string key, int defaultValue)
        {
            try
            {
                return Preferences.Get(key, defaultValue);
            }
            catch (InvalidCastException)
            {
                logManager.Error($"Value {key} couldn't be parsed to int.");

                Preferences.Set(key, defaultValue);
                return defaultValue;
            }
        }

        public void AddOrUpdate(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        public void AddOrUpdate(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public void AddOrUpdate(string key, int value)
        {
            Preferences.Set(key, value);
        }

        public void Remove(string key)
        {
            Preferences.Remove(key);
        }
    }
}
