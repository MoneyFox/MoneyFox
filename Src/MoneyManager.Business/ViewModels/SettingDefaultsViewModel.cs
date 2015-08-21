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
            => ServiceLocator.Current.GetInstance<IRepository<Account>>().Data;

        private readonly SettingDataAccess settings;
        private readonly IRepository<Account> accountRepository;

        public SettingDefaultsViewModel(SettingDataAccess settings, IRepository<Account> accountRepository)
        {
            this.settings = settings;
            this.accountRepository = accountRepository;
        }

        public Account DefaultAccount
        {
            get
            {
                return settings.DefaultAccount == -1
                    ? accountRepository.Selected
                    : AllAccounts.FirstOrDefault(x => x.Id == settings.DefaultAccount);
            }
            set { ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultAccount = value.Id; }
        }
    }
}