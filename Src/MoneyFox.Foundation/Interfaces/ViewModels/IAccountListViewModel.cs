using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces.ViewModels
{
    public interface IAccountListViewModel
    {
        ObservableCollection<Account> AllAccounts { get; set; }

        Account SelectedAccount { get; set; }

        IBalanceViewModel BalanceViewModel { get; }

        RelayCommand LoadedCommand { get; }
    }
}