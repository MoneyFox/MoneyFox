using Windows.Storage;

namespace MoneyFox.Uwp.Helpers
{
    public static class SettingsStorageExtensions
    {
        public static void SaveString(this ApplicationDataContainer settings, string key, string value)
        {
            settings.Values[key] = value;
        }

        public static T ReadAsync<T>(this ApplicationDataContainer settings, string key)
        {
            object obj = null;

            if(settings.Values.TryGetValue(key, out obj))
                return (T) obj;

            return default;
        }
    }
}
