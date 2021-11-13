using Windows.Storage;

#nullable enable
namespace MoneyFox.Uwp.Helpers
{
    public static class SettingsStorageExtensions
    {
        public static void SaveString(this ApplicationDataContainer settings, string key, string value)
            => settings.Values[key] = value;

        public static T Read<T>(this ApplicationDataContainer settings, string key)
        {

            if(settings.Values.TryGetValue(key, out object obj))
            {
                return (T)obj;
            }

            return default;
        }
    }
}
