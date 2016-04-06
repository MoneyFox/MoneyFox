using System.Collections.ObjectModel;
using MoneyManager.Foundation.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Foundation.Interfaces.ViewModels
{
    public interface IAccountListViewModel
    {
        ObservableCollection<Account>  AllAccounts { get; set; }

        Account SelectedAccount { get; set; }

        IBalanceViewModel BalanceViewModel { get; }

        MvxCommand LoadedCommand { get; }
    }
}
