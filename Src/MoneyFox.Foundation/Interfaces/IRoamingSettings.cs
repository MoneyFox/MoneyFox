namespace MoneyManager.Foundation.Interfaces
{
    /// <summary>
    ///     Grants CRUD operations to the settings on each plattform
    /// </summary>
    public interface IRoamingSettings
    {
        /// <summary>
        ///     Adds or updates the key value pair to the settings.
        /// </summary>
        /// <param name="key">Key of the setting.</param>
        /// <param name="value">Value of the setting.</param>
        void AddOrUpdateValue(string key, object value);

        /// <summary>
        ///     Reads the value out of the  settings.
        /// </summary>
        /// <typeparam name="TValueType">Defaultvalue Type.</typeparam>
        /// <param name="key">Key of the setting.</param>
        /// <param name="defaultValue">Value of the setting.</param>
        /// <returns>Value from the settings.</returns>
        TValueType GetValueOrDefault<TValueType>(string key, TValueType defaultValue);
    }
}