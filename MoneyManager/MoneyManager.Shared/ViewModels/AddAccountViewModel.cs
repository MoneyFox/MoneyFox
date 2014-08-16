using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PropertyChanged;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    public class AddAccountViewModel : ViewModelBase
    {
        public bool IsEdit { get; set; }

        public Account SelectedAccount
        {
            get { return new ViewModelLocator().AccountDataAccess.SelectedAccount; }
        }

        public RelayCommand AddAccountCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public AddAccountViewModel()
        {
            AddAccountCommand = new RelayCommand(AddAccount);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void AddAccount()
        {
            ServiceLocator.Current.GetInstance<AccountDataAccess>().Save(SelectedAccount);
            ((Frame)Window.Current.Content).GoBack();
        }

        private void Cancel()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}