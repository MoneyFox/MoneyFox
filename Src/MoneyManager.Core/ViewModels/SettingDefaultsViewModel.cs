using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<Account> AllAccounts => accountRepository.Data;

        public Account DefaultAccount
        {
            get
            {
                return settings.DefaultAccount == -1
                    ? accountRepository.Selected
                    : AllAccounts.FirstOrDefault(x => x.Id == settings.DefaultAccount);
            }
            set { settings.DefaultAccount = value.Id; }
        }
    }
}