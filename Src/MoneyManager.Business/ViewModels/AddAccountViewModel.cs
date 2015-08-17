using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddAccountViewModel : ViewModelBase
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly SettingDataAccess _settings;

        public AddAccountViewModel(IRepository<Account> accountRepository,
            SettingDataAccess settings)
        {
            _settings = settings;
            _accountRepository = accountRepository;
        }

        public bool IsEdit { get; set; }

        public Account SelectedAccount
        {
            get { return _accountRepository.Selected; }
            set { _accountRepository.Selected = value; }
        }        

        public void Save()
        {
            _accountRepository.Save(_accountRepository.Selected);
            ((Frame) Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            ((Frame) Window.Current.Content).GoBack();
        }
    }
}