using MoneyManager.Core.Manager;
using MoneyManager.DataAccess;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.ViewModels
{
    public class SettingDefaultsViewModel
    {
        private readonly DefaultManager defaultManager;
        private readonly SettingDataAccess settings;

        public SettingDefaultsViewModel(DefaultManager defaultManager, SettingDataAccess settings)
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