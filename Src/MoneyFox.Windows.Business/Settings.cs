using System;
using Windows.Storage;
using MoneyFox.Business;
using Newtonsoft.Json;

namespace MoneyFox.Windows.Business
{
    public class Settings : ISettings
    {
        private static ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;
        private static ApplicationDataContainer RoamingSettings => ApplicationData.Current.RoamingSettings;

        public T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false)
        {
            return GetValue(!roaming ? LocalSettings : RoamingSettings, key, defaultValue);
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false)
        {
            return AddOrUpdateValue(!roaming ? LocalSettings : RoamingSettings, key, value);
        }

        public bool DeleteValue(string key, bool roaming = false)
        {
            return DeleteValue(!roaming ? LocalSettings : RoamingSettings, key);
        }

        public bool Contains(string key, bool roaming = false)
        {
            return Contains(!roaming ? LocalSettings : RoamingSettings, key);
        }

        public bool ClearAllValues(bool roaming = false)
        {
            return ClearAllValues(!roaming ? LocalSettings : RoamingSettings);
        }

        private static T GetValue<T>(ApplicationDataContainer container, string key, T defaultValue = default(T))
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            object value;

            if (container.Values.TryGetValue(key, out value))
            {
                var json = (string) value;
                if (string.IsNullOrEmpty(json)) return defaultValue;

                var deserializedValue = JsonConvert.DeserializeObject<T>(json);
                return deserializedValue;
            }

            return defaultValue;
        }

        private static bool AddOrUpdateValue<T>(ApplicationDataContainer container, string key, T value = default(T))
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            var serializedValue = JsonConvert.SerializeObject(value);

            if (container.Values.ContainsKey(key))
            {
                container.Values[key] = serializedValue;
                return true;
            }

            container.Values.Add(key, serializedValue);

            return true;
        }

        private static bool DeleteValue(ApplicationDataContainer container, string key)
        {
            if (container.Values.ContainsKey(key))
            {
                container.Values.Remove(key);
                return true;
            }

            return false;
        }

        private static bool Contains(ApplicationDataContainer container, string key)
        {
            return container.Values.ContainsKey(key);
        }

        private static bool ClearAllValues(ApplicationDataContainer container)
        {
            container.Values.Clear();
            return true;
        }
    }
}