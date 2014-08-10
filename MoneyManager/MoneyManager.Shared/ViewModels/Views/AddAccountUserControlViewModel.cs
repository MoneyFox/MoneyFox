using GalaSoft.MvvmLight;
using MoneyManager.Models;

namespace MoneyManager.ViewModels.Views
{
    public class AddAccountUserControlViewModel : ViewModelBase
    {
        public Account SelectedAccount
        {
            get { return new ViewModelLocator().AccountViewModel.SelectedAccount; }
            set { new ViewModelLocator().AccountViewModel.SelectedAccount = value; }
        }

        public AddAccountUserControlViewModel()
        {
            SelectedAccount = new Account();
        }
    }
}