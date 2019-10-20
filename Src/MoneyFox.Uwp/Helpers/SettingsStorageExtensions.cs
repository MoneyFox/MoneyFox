using System.Threading.Tasks;
using Windows.Storage;

namespace MoneyFox.Uwp.Helpers
{
    public static class SettingsStorageExtensions
    {
        public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value)
        {
            settings.SaveString(key, await Json.StringifyAsync(value));
        }

        public static void SaveString(this ApplicationDataContainer settings, string key, string value)
        {
            settings.Values[key] = value;
        }

        public static async Task<T> ReadAsync<T>(this ApplicationDataContainer settings, string key)
        {
            object obj = null;

            if (settings.Values.TryGetValue(key, out obj)) return await Json.ToObjectAsync<T>((string) obj);

            return default;
        }
    }
}
