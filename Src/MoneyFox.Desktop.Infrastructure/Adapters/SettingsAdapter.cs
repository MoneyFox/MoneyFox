using MoneyFox.Application.Common.Adapters;
using NLog;
using System;

namespace MoneyFox.Desktop.Infrastructure.Adapters
{
    public class SettingsAdapter : ISettingsAdapter
    {
            private readonly Logger logManager = LogManager.GetCurrentClassLogger();

            public bool GetValue(string key, bool defaultValue)
            {
                try
                {
                    return Preferences.Get(key, defaultValue);
                }
                catch(InvalidCastException)
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
                catch(InvalidCastException)
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
                catch(InvalidCastException)
                {
                    logManager.Error($"Value {key} couldn't be parsed to int.");

                    Preferences.Set(key, defaultValue);
                    return defaultValue;
                }
            }

            public void AddOrUpdate(string key, bool value) => Preferences.Set(key, value);

            public void AddOrUpdate(string key, string value) => Preferences.Set(key, value);

            public void AddOrUpdate(string key, int value) => Preferences.Set(key, value);

            public void Remove(string key) => Preferences.Remove(key);

    }
}
