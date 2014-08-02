using GalaSoft.MvvmLight;
using MoneyManager.Models;
using MoneyManager.ViewModels.Data;

namespace MoneyManager.ViewModels.Views
{
    public class MainViewModel : ViewModelBase
    {
        public AccountViewModel AccountViewModel
        {
            get { return new ViewModelLocator().AccountViewModel; }
        }

        public Account SelectedAccount
        {
            get { return new ViewModelLocator().AccountViewModel.SelectedAccount; }
            set { new ViewModelLocator().AccountViewModel.SelectedAccount = value; }
        }

        public FinancialTransaction SelectedTransaction
        {
            get { return new ViewModelLocator().TransactionViewModel.SelectedTransaction; }
            set { new ViewModelLocator().TransactionViewModel.SelectedTransaction = value; }
        }

        public SettingViewModel SettingViewModel
        {
            get { return new ViewModelLocator().Settings; }
        }
    }
}