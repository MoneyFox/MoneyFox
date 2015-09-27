using System;
using System.Globalization;
using Windows.Storage;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows
{
    public class RoamingSettings : IRoamingSettings
    {
        public void AddOrUpdateValue(string key, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[key] = value;
        }

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