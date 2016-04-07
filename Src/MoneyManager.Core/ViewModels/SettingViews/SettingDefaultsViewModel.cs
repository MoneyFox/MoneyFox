using MoneyManager.DataAccess;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.ViewModels.SettingViews
{
    public class SettingDefaultsViewModel
    {
        private readonly IDefaultManager defaultManager;
        private readonly SettingDataAccess settings;

        private bool _isDarkThemeEnabled = false;

        public SettingDefaultsViewModel(IDefaultManager defaultManager, SettingDataAccess settings)
        {
            this.defaultManager = defaultManager;
            this.settings = settings;
        }

        public Account DefaultAccount
        {
            get { return defaultManager.GetDefaultAccount(); }
            set { settings.DefaultAccount = value.Id; }
        }
    }
}