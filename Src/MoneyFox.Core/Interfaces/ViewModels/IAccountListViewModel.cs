using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.DatabaseModels;

namespace MoneyFox.Core.Interfaces.ViewModels
{
    public interface IAccountListViewModel
    {
        ObservableCollection<Account> AllAccounts { get; set; }

        Account SelectedAccount { get; set; }

        IBalanceViewModel BalanceViewModel { get; }

        RelayCommand LoadedCommand { get; }
    }
}