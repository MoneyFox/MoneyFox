using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace MoneyManager.DataAccess.DataAccess
{
    internal class SettingDataAccess : INotifyPropertyChanged
    {
        private const string DbVersionKeyname = "DbVersion";

        private const int DbVersionKeydefault = 1;

        #region Properties

        public int Dbversion
        {
            get { return GetValueOrDefault(DbVersionKeyname, DbVersionKeydefault); }
            set
            {
                AddOrUpdateValue(DbVersionKeyname, value);
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
                value = (valueType)Convert.ChangeType(setting, typeof(valueType), CultureInfo.InvariantCulture);
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