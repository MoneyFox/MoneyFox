using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class AddAccountViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;

        public AddAccountViewModel(IRepository<Account> accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public bool IsEdit { get; set; }

        public Account SelectedAccount
        {
            get { return accountRepository.Selected; }
            set { accountRepository.Selected = value; }
        }

        public void Save()
        {
            accountRepository.Save(accountRepository.Selected);
            Close(this);
        }

        public void Cancel()
        {
            Close(this);
        }
    }
}