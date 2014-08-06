using GalaSoft.MvvmLight;
using MoneyManager.Models;
using MoneyManager.ViewModels.Data;

namespace MoneyManager.ViewModels.Views
{
    public class AddAccountUserControlViewModel : ViewModelBase
    {
        public Account SelectedAccount
        {
            get { return new ViewModelLocator().AccountViewModel.SelectedAccount; }
        }

        public AddAccountUserControlViewModel()
        {
            SelectedAccount = new Account();
        }
    }
}