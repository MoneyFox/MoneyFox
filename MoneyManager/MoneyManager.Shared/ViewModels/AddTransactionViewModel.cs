using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PropertyChanged;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    public class AddTransactionViewModel
    {
        public FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
        }

        public AddTransactionUserControlViewModel AddTransactionControl
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionUserControlViewModel>(); }
        }

        public string Title
        {
            get
            {
                return AddTransactionControl.IsEdit
                    ? Utilities.GetTranslation("EditTitle")
                    : Utilities.GetTranslation("AddTitle");
            }
        }

        public RelayCommand AddTransactionCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public AddTransactionViewModel()
        {
            AddTransactionCommand = new RelayCommand(AddTransaction);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void AddTransaction()
        {
            ServiceLocator.Current.GetInstance<TransactionDataAccess>().Save(SelectedTransaction);
            ((Frame)Window.Current.Content).GoBack();
        }

        private void Cancel()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}