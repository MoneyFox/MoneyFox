using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Core.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    public class SettingDefaultsViewModel
    {
        private readonly IRepository<Account> accountRepository;

        private readonly SettingDataAccess settings;

        public SettingDefaultsViewModel(SettingDataAccess settings, IRepository<Account> accountRepository)
        {
            this.settings = settings;
            this.accountRepository = accountRepository;
        }

        public ObservableCollection<Account> AllAccounts
            => ServiceLocator.Current.GetInstance<IRepository<Account>>().Data;

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