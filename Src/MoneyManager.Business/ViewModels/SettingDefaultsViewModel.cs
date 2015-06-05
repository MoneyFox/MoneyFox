using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.ViewModels
{
    public class SettingDefaultsViewModel
    {
        public ObservableCollection<Account> AllAccounts
        {
            get { return ServiceLocator.Current.GetInstance<IAccountRepository>().Data; }
        }

        private SettingDataAccess settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        private IAccountRepository AccountRepository
        {
            get { return ServiceLocator.Current.GetInstance<IAccountRepository>(); }
        }

        public Account DefaultAccount
        {
            get
            {
                return settings.DefaultAccount == -1
                    ? AccountRepository.Selected
                    : AllAccounts.First(x => x.Id == settings.DefaultAccount);
            }
            set { ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultAccount = value.Id; }
        }
    }
}