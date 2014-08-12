using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;

namespace MoneyManager.ViewModels
{
    public class AccountListUserControlViewModel : ViewModelBase
    {
        public AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public SettingDataAccess SettingDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); ; }
        }

        public RelayCommand LoadAccountsCommand { get; private set; }

        public AccountListUserControlViewModel()
        {
            LoadAccountsCommand = new RelayCommand(LoadAccounts);
        }

        private void LoadAccounts()
        {
            AccountDataAccess.LoadList();
        }

        public void Delete(Account account)
        {
            ServiceLocator.Current.GetInstance<AccountDataAccess>().Delete(account);
        }
    }
}