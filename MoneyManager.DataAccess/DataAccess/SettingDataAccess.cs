#region

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Globalization;
using Windows.Storage;
using PropertyChanged;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class SettingDataAccess : INotifyPropertyChanged {
        private const string DefaultCurrencyKeyname = "DefaultCurrency";
        private const string DefaultAccountKeyname = "DefaultAccount";

        private const int DefaultAccountKeydefault = -1;

        #region Properties

        public string DefaultCurrency {
            get { return GetValueOrDefault(DefaultCurrencyKeyname, new GeographicRegion().CurrenciesInUse.First()); }
            set {
                AddOrUpdateValue(DefaultCurrencyKeyname, value);
                OnPropertyChanged();
            }
        }

        public int DefaultAccount {
            get { return GetValueOrDefault(DefaultAccountKeyname, DefaultAccountKeydefault); }
            set {
                AddOrUpdateValue(DefaultAccountKeyname, value);
                OnPropertyChanged();
            }
        }

        #endregion Properties

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddOrUpdateValue(string key, object value) {
            ApplicationData.Current.RoamingSettings.Values[key] = value;
        }

        private valueType GetValueOrDefault<valueType>(string key, valueType defaultValue) {
            valueType value;

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key)) {
                object setting = ApplicationData.Current.RoamingSettings.Values[key];
                value = (valueType) Convert.ChangeType(setting, typeof (valueType), CultureInfo.InvariantCulture);
            } else {
                value = defaultValue;
            }
            return value;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}