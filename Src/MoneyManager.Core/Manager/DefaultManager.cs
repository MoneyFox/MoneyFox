using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.DataAccess;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Manager
{
    public class DefaultManager
    {
        private readonly IRepository<Account> accountRepository;
        private readonly SettingDataAccess settings;

        public DefaultManager(IRepository<Account> accountRepository, SettingDataAccess settings)
        {
            this.accountRepository = accountRepository;
            this.settings = settings;
        }

        public Account GetDefaultAccount()
        {
            if (accountRepository.Selected != null)
            {
                return accountRepository.Selected;
            }

            if (accountRepository.Data == null)
            {
                accountRepository.Data = new ObservableCollection<Account>();
            }

            if (accountRepository.Data.Any() && settings.DefaultAccount != -1)
            {
                return accountRepository.Data.FirstOrDefault(x => x.Id == settings.DefaultAccount);
            }

            return accountRepository.Data.FirstOrDefault();
        }
    }
}