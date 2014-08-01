using GalaSoft.MvvmLight;
using MoneyManager.Models;

namespace MoneyManager.ViewModels.Views
{
    public class AddAccountViewModel : ViewModelBase
    {
        public bool IsEditMode { get; set; }

        public Account SelectedAccount
        {
            get { return new ViewModelLocator().AccountViewModel.SelectedAccount; }
        }
    }
}