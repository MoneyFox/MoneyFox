using System;
using Xamarin.Essentials;

namespace MoneyFox.Business.Adapter
{
    public interface ISettingsAdapter
    {
        bool GetValue(string key, bool defaultValue);
        string GetValue(string key, string defaultValue);
        int GetValue(string key, int defaultValue);

        void AddOrUpdate(string key, bool value);
        void AddOrUpdate(string key, string value);
        void AddOrUpdate(string key, int value);

        void Remove(string key);
    }

    public class SettingsAdapter : ISettingsAdapter
    {
        public bool GetValue(string key, bool defaultValue)
        {
            try
            {
                return Preferences.Get(key, defaultValue);
            }
            catch (InvalidCastException)
            {
                Preferences.Set(key, false);
                return false;
            }
        }

        public string GetValue(string key, string defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public int GetValue(string key, int defaultValue)
        {
            return Preferences.Get(key, defaultValue);
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
