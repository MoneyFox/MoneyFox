namespace MoneyFox.Mobile.Infrastructure.Adapters
{

    using System;
    using Core.Interfaces;
    using NLog;
    using Xamarin.Essentials;

    public class SettingsAdapter : ISettingsAdapter
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public bool GetValue(string key, bool defaultValue)
        {
            try
            {
                return Preferences.Get(key: key, defaultValue: defaultValue);
            }
            catch (InvalidCastException)
            {
                logManager.Error($"Value {key} couldn't be parsed to bool.");
                Preferences.Set(key: key, value: defaultValue);

                return defaultValue;
            }
        }

        public string GetValue(string key, string defaultValue)
        {
            try
            {
                return Preferences.Get(key: key, defaultValue: defaultValue);
            }
            catch (InvalidCastException)
            {
                logManager.Error($"Value {key} couldn't be parsed to string.");
                Preferences.Set(key: key, value: defaultValue);

                return defaultValue;
            }
        }

        public int GetValue(string key, int defaultValue)
        {
            try
            {
                return Preferences.Get(key: key, defaultValue: defaultValue);
            }
            catch (InvalidCastException)
            {
                logManager.Error($"Value {key} couldn't be parsed to int.");
                Preferences.Set(key: key, value: defaultValue);

                return defaultValue;
            }
        }

        public void AddOrUpdate(string key, bool value)
        {
            Preferences.Set(key: key, value: value);
        }

        public void AddOrUpdate(string key, string value)
        {
            Preferences.Set(key: key, value: value);
        }

        public void AddOrUpdate(string key, int value)
        {
            Preferences.Set(key: key, value: value);
        }

        public void Remove(string key)
        {
            Preferences.Remove(key);
        }
    }

}
