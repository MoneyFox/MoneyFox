using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Globalization;
using Windows.Storage;
using PropertyChanged;

namespace MoneyManager.Business.DataAccess
{
    [ImplementPropertyChanged]
    public class SettingDataAccess : INotifyPropertyChanged
    {
        private const string DEFAULT_CURRENCY_KEYNAME = "DefaultCurrency";
        private const string DEFAULT_ACCOUNT_KEYNAME = "DefaultAccount";
        private const string SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME = "ShowCashFlowOnMainTile";
        private const int DEFAULT_ACCOUNT_KEYDEFAULT = -1;
        private const bool SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT = false;
        public event PropertyChangedEventHandler PropertyChanged;

        private void AddOrUpdateValue(string key, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[key] = value;
        }

        private TValueType GetValueOrDefault<TValueType>(string key, TValueType defaultValue)
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties

        public string DefaultCurrency
        {
            get { return GetValueOrDefault(DEFAULT_CURRENCY_KEYNAME, new GeographicRegion().CurrenciesInUse.First()); }
            set
            {
                AddOrUpdateValue(DEFAULT_CURRENCY_KEYNAME, value);
                OnPropertyChanged();
            }
        }

        public int DefaultAccount
        {
            get { return GetValueOrDefault(DEFAULT_ACCOUNT_KEYNAME, DEFAULT_ACCOUNT_KEYDEFAULT); }
            set
            {
                AddOrUpdateValue(DEFAULT_ACCOUNT_KEYNAME, value);
                OnPropertyChanged();
            }
        }

        public bool ShowCashFlowOnMainTile
        {
            get
            {
                return GetValueOrDefault(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT);
            }
            set
            {
                AddOrUpdateValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, value);
                OnPropertyChanged();
            }
        }

        #endregion Properties
    }
}