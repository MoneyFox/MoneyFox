using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Droid
{
    public class RoamingSettings : IRoamingSettings
    {
        public void AddOrUpdateValue(string key, object value)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(key, Convert.ToString(value));
            editor.Apply();
        }

        public TValueType GetValueOrDefault<TValueType>(string key, TValueType defaultValue)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            string value = prefs.GetString(key, null);

            return value != null
                ? (TValueType)Convert.ChangeType(value, typeof(TValueType))
                : defaultValue;
        }
    }
}