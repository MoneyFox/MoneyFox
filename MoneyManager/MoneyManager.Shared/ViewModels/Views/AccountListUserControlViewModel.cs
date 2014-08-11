using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyManager.ViewModels.Data;

namespace MoneyManager.ViewModels.Views
{
    public class AccountListUserControlViewModel : ViewModelBase
    {
        public AccountViewModel AccountViewModel
        {
            get { return new ViewModelLocator().AccountViewModel; }
        }

        public SettingViewModel SettingViewModel
        {
            get { return new ViewModelLocator().Settings; }
        }

        public RelayCommand LoadAccountsCommand { get; private set; }

        public AccountListUserControlViewModel()
        {
            LoadAccountsCommand = new RelayCommand(LoadAccounts);
        }

        private void LoadAccounts()
        {
            new ViewModelLocator().AccountViewModel.LoadList();
        }
    }
}