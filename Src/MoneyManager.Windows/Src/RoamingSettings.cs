using System;
using System.Globalization;
using Windows.Storage;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows
{
    /// <summary>
    ///     Grants CRUD operations to the roaming settings on windows.
    /// </summary>
    public class RoamingSettings : IRoamingSettings
    {
        /// <summary>
        ///     Adds or updates the key value pair to the roaming settings.
        /// </summary>
        /// <param name="key">Key of the setting.</param>
        /// <param name="value">Value of the setting.</param>
        public void AddOrUpdateValue(string key, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[key] = value;
        }

        /// <summary>
        ///     Reads the value out of the roaming settings.
        /// </summary>
        /// <typeparam name="TValueType">Defaultvalue Type.</typeparam>
        /// <param name="key">Key of the setting.</param>
        /// <param name="defaultValue">Value of the setting.</param>
        /// <returns>Value from the RoamingSetting</returns>
        public TValueType GetValueOrDefault<TValueType>(string key, TValueType defaultValue)
        {
            TValueType value;

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
            {
                var setting = ApplicationData.Current.RoamingSettings.Values[key];
                value = (TValueType) Convert.ChangeType(setting, typeof (TValueType), CultureInfo.InvariantCulture);
            }
            else
            {
                value = defaultValue;
            }

            return value;
        }
    }
}