using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Manager {
    //TODO: Refactor to helper class
    public class DefaultManager : IDefaultManager {
        private readonly IAccountRepository accountRepository;

        public DefaultManager(IAccountRepository accountRepository) {
            this.accountRepository = accountRepository;
        }

        public Account GetDefaultAccount() {

            if (accountRepository.Data == null) {
                accountRepository.Data = new ObservableCollection<Account>();
            }

            if (accountRepository.Data.Any() && SettingsHelper.DefaultAccount != -1) {
                return accountRepository.Data.FirstOrDefault(x => x.Id == SettingsHelper.DefaultAccount);
            }

            return accountRepository.Data.FirstOrDefault();
        }
    }
}