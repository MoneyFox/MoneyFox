using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public Account SelectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
            set { ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount = value; }
        }

        public AddTransactionUserControlViewModel AddTransactionControl
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionUserControlViewModel>(); }
        }

        public RelayCommand<string> AddTransactionCommand { get; private set; }

        public MainPageViewModel()
        {
            AddTransactionCommand = new RelayCommand<string>(AddTransaction);
        }

        private void AddTransaction(string transactionType)
        {
            AddTransactionControl.TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), transactionType);
            ((Frame)Window.Current.Content).Navigate(typeof(AddTransaction));
        }
    }
}