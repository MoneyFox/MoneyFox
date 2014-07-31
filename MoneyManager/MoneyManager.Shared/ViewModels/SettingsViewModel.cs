using MoneyManager.Annotations;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace MoneyManager.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private const string DefaultCurrencyStepKeyName = "DefaultCurrency";

        private const string DefaultCurrencyStepKeydefault = "USD";

        #region Properties

        public string DoseIncrementStep
        {
            get
            {
                return GetValueOrDefault(DefaultCurrencyStepKeyName, DefaultCurrencyStepKeydefault);
            }
            set
            {
                AddOrUpdateValue(DefaultCurrencyStepKeyName, value);
                OnPropertyChanged();
            }
        }

        #endregion Properties

        private void AddOrUpdateValue(string key, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[key] = value;
        }

        private valueType GetValueOrDefault<valueType>(string key, valueType defaultValue)
        {
            valueType value;

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
            {
                var setting = ApplicationData.Current.RoamingSettings.Values[key];
                value = (valueType)Convert.ChangeType(setting, typeof(valueType), CultureInfo.InvariantCulture);
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}