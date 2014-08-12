using GalaSoft.MvvmLight;
using MoneyManager.Models;

namespace MoneyManager.ViewModels
{
    public class AddAccountUserControlViewModel : ViewModelBase
    {
        public Account SelectedAccount
        {
            get { return new ViewModelLocator().AccountDataAccess.SelectedAccount; }
            set { new ViewModelLocator().AccountDataAccess.SelectedAccount = value; }
        }

        public AddAccountUserControlViewModel()
        {
            SelectedAccount = new Account();
        }
    }
}