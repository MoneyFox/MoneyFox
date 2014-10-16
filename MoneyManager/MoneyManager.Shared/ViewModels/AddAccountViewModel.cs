using System.Globalization;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using PropertyChanged;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    public class AddAccountViewModel : ViewModelBase
    {
        public bool IsEdit { get; set; }

        public Account SelectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
            set { ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount = value; }
        }

        public void Save()
        {
            if (IsEdit)
            {
                ServiceLocator.Current.GetInstance<AccountDataAccess>().Update(SelectedAccount);
            }
            else
            {
                ServiceLocator.Current.GetInstance<AccountDataAccess>().Save(SelectedAccount);
            }
            ((Frame)Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}