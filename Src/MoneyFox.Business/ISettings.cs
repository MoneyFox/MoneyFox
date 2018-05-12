namespace MoneyFox.Business
{
    public interface ISettings
    {
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of T (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for setting</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="roaming">Roam settings (only for WindowsCommon)</param>
        /// <returns></returns>
        T GetValue<T>(string key, T defaultValue = default(T), bool roaming = false);

        /// <summary>
        /// Adds or updates the value
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="roaming">Roam settings (only for WindowsCommon)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        bool AddOrUpdateValue<T>(string key, T value = default(T), bool roaming = false);

        /// <summary>
        /// Delete value from settings
        /// </summary>
        /// <param name="key">Key for stored value</param>
        /// <param name="roaming">Roam settings (only for WindowsCommon)</param>
        /// <returns>Returns if anything was deleted</returns>
        bool DeleteValue(string key, bool roaming = false);

        /// <summary>
        /// Check if Settings contains a value for a key
        /// </summary>
        /// <param name="key">Key to for value</param>
        /// <param name="roaming">Roam settings (only for WindowsCommon)</param>
        /// <returns>Returns true if a value for key is contained in Settings or false if not</returns>
        bool Contains(string key, bool roaming = false);

        /// <summary>
        /// Delete everything!
        /// </summary>
        /// <param name="roaming">Roam settings (only for WindowsCommon)</param>
        /// <returns>Returns if operation was successful</returns>
        bool ClearAllValues(bool roaming = false);
    }
}
