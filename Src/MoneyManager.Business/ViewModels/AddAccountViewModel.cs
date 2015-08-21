using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddAccountViewModel : ViewModelBase
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
            ((Frame) Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            ((Frame) Window.Current.Content).GoBack();
        }
    }
}