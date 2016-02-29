using MoneyFox.DataAccess;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.ViewModels
{
    //TODO: Check if somewhere used
    public class SettingDefaultsViewModel
    {
        private readonly IDefaultManager defaultManager;
        private readonly SettingDataRepository settings;

        public SettingDefaultsViewModel(IDefaultManager defaultManager, SettingDataRepository settings)
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