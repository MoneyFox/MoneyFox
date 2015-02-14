using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.Business.ViewModels {
    public class SettingDefaultsViewModel {
        public ObservableCollection<Account> AllAccounts {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        private SettingDataAccess settings {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        private AccountDataAccess accountData {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public Account DefaultAccount {
            get {
                return settings.DefaultAccount == -1
                    ? accountData.SelectedAccount
                    : AllAccounts.First(x => x.Id == settings.DefaultAccount);
            }
            set { ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultAccount = value.Id; }
        }
    }
}