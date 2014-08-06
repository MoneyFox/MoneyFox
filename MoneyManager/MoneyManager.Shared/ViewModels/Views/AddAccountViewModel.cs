using GalaSoft.MvvmLight;
using MoneyManager.Models;
using MoneyManager.ViewModels.Data;

namespace MoneyManager.ViewModels.Views
{
  public class AddAccountViewModel : ViewModelBase
  {
    public Account SelectedAccount
    {
        get { return new ViewModelLocator().AccountViewModel.SelectedAccount; }
    }
  }
}