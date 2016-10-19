using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IAccountListViewModel
    {
        ObservableCollection<AccountViewModel> AllAccounts { get; set; }

        AccountViewModel SelectedAccountViewModel { get; set; }

        IBalanceViewModel BalanceViewModel { get; }

        MvxCommand LoadedCommand { get; }
    }
}