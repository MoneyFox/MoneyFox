using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddAccountViewModel : ViewModelBase
    {
        private readonly IRepository<Account> accountRepository;
        private readonly INavigationService navigationService;

        public AddAccountViewModel(IRepository<Account> accountRepository, INavigationService navigationService)
        {
            this.accountRepository = accountRepository;
            this.navigationService = navigationService;
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
            navigationService.GoBack();
        }

        public void Cancel()
        {
            navigationService.GoBack();
        }
    }
}