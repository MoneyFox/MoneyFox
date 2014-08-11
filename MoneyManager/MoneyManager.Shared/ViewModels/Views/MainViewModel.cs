using GalaSoft.MvvmLight;
using MoneyManager.Models;

namespace MoneyManager.ViewModels.Views
{
    public class MainViewModel : ViewModelBase
    {
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
    }
}