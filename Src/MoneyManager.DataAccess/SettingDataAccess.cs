using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoneyManager.Foundation.Interfaces;
using PropertyChanged;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class SettingDataAccess : INotifyPropertyChanged
    {
        // Settings Names
        private const string DEFAULT_ACCOUNT_KEYNAME = "DefaultAccount";
        private const string SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME = "ShowCashFlowOnMainTile";

        // Default Settings
        private const int DEFAULT_ACCOUNT_KEYDEFAULT = -1;
        private const bool SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT = false;

        private readonly IRoamingSettings roamingSettings;

        public SettingDataAccess(IRoamingSettings roamingSettings)
        {
            this.roamingSettings = roamingSettings;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddOrUpdateValue(string key, object value)
        {
            roamingSettings.AddOrUpdateValue(key, value);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Properties

        public int DefaultAccount
        {
            get { return roamingSettings.GetValueOrDefault(DEFAULT_ACCOUNT_KEYNAME, DEFAULT_ACCOUNT_KEYDEFAULT); }
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
                return roamingSettings.GetValueOrDefault(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME,
                    SHOW_CASH_FLOW_ON_MAIN_TILE_KEYDEFAULT);
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