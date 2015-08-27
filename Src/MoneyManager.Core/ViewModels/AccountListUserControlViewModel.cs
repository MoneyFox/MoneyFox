using System.Collections.ObjectModel;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    public class AccountListUserControlViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;

        public AccountListUserControlViewModel(IRepository<Account> accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public ObservableCollection<Account> AllAccounts
        {
            get { return accountRepository.Data; }
            set { accountRepository.Data = value; }
        }

        public void Delete(Account item)
        {
            accountRepository.Delete(item);
        }
    }
}