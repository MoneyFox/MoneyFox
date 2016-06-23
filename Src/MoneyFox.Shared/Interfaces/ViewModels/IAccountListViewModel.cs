using System.Collections.ObjectModel;
using MoneyFox.Shared.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.Interfaces.ViewModels {
    public interface IAccountListViewModel {
        ObservableCollection<Account> AllAccounts { get; set; }

        Account SelectedAccount { get; set; }

        IBalanceViewModel BalanceViewModel { get; }

        MvxCommand LoadedCommand { get; }
    }
}