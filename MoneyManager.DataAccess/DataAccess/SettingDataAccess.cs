using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.Storage;
using PropertyChanged;

namespace MoneyManager.DataAccess.DataAccess
{
    [ImplementPropertyChanged]
    internal class SettingDataAccess : INotifyPropertyChanged
    {
        private const string DefaultCurrencyKeyname = "DefaultCurrency";

        private const string DefaultCurrencyKeydefault = "USD";

        #region Properties

        public string DefaultCurrency
        {
            get { return GetValueOrDefault(DefaultCurrencyKeyname, DefaultCurrencyKeydefault); }
            set
            {
                AddOrUpdateValue(DefaultCurrencyKeyname, value);
                OnPropertyChanged();
            }
        }

        #endregion Properties

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddOrUpdateValue(string key, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[key] = value;
        }

        private valueType GetValueOrDefault<valueType>(string key, valueType defaultValue)
        {
            valueType value;

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
            {
                object setting = ApplicationData.Current.RoamingSettings.Values[key];
                value = (valueType) Convert.ChangeType(setting, typeof (valueType), CultureInfo.InvariantCulture);
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}