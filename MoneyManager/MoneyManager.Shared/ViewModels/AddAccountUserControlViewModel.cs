using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;

namespace MoneyManager.ViewModels
{
    public class AddAccountUserControlViewModel : ViewModelBase
    {
        public Account SelectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
            set { ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount = value; }
        }
    }
}