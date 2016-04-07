using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.ViewModels
{
    //TODO: Check if somewhere used
    public class SettingDefaultsViewModel
    {
        private readonly IDefaultManager defaultManager;
        private readonly SettingDataAccess settings;

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