using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace MoneyManager.ViewModels.Views
{
    public class AddAccountViewModel : ViewModelBase
    {
        public RelayCommand AddAccountCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public AddAccountViewModel()
        {
            AddAccountCommand = new RelayCommand(AddAccount);
            CancelCommand = new RelayCommand(Cancel);
        }

        public Account SelectedAccount
        {
            get { return new ViewModelLocator().AccountDataAccess.SelectedAccount; }
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