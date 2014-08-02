using GalaSoft.MvvmLight;
using MoneyManager.Models;
using MoneyManager.ViewModels.Data;
using System.Collections.ObjectModel;

namespace MoneyManager.ViewModels.Views
{
    public class AccountListViewModel : ViewModelBase
    {
        public ObservableCollection<Account> AllAccounts
        {
            get { return new ViewModelLocator().AccountViewModel.AllAccounts; }
        }

        public AccountViewModel AccountViewModel
        {
            get { return new ViewModelLocator().AccountViewModel; }
        }

        public SettingViewModel SettingViewModel
        {
            get { return new ViewModelLocator().Settings; }
        }
    }
}