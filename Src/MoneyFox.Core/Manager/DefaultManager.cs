using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Manager
{
    public class DefaultManager : IDefaultManager
    {
        private readonly IAccountRepository accountRepository;
        private readonly SettingDataRepository settings;

        public DefaultManager(IAccountRepository accountRepository, SettingDataRepository settings)
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