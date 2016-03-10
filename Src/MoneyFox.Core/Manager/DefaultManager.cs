using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Core.SettingAccess;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Interfaces;

namespace MoneyFox.Core.Manager
{
    public class DefaultManager : IDefaultManager
    {
        private readonly IAccountRepository accountRepository;

        public DefaultManager(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
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

            if (accountRepository.Data.Any() && Settings.DefaultAccount != -1)
            {
                return accountRepository.Data.FirstOrDefault(x => x.Id == Settings.DefaultAccount);
            }

            return accountRepository.Data.FirstOrDefault();
        }
    }
}