using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.ViewModels
{
    public class AccountListUserControlViewModel : ViewModelBase
    {
        public AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public Account SelectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
            set { ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount = value; }
        }

        public Account SelectedItem { get; set; }

        public SettingDataAccess SettingDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); ; }
        }

        public RelayCommand LoadAccountsCommand { get; private set; }

        public RelayCommand GoToTransactionListCommand { get; private set; }

        public AccountListUserControlViewModel()
        {
            LoadAccountsCommand = new RelayCommand(LoadAccounts);
            GoToTransactionListCommand = new RelayCommand(GoToTransactionList);
        }

        private void LoadAccounts()
        {
            AccountDataAccess.LoadList();
        }

        private void GoToTransactionList()
        {
            if (SelectedItem != null)
            {
                SelectedAccount = SelectedItem;
                ((Frame)Window.Current.Content).Navigate(typeof(TransactionList));
                SelectedItem = null;
            }
        }

        public void Delete(Account account)
        {
            ServiceLocator.Current.GetInstance<AccountDataAccess>().Delete(account);
        }
    }
}