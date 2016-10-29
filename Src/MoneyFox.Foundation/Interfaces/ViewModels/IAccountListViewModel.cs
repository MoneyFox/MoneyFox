using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IAccountListViewModel
    {
        ObservableCollection<AccountViewModel> AllAccounts { get; set; }

        IBalanceViewModel BalanceViewModel { get; }

        MvxCommand LoadedCommand { get; }

        MvxCommand<AccountViewModel> EditAccountCommand { get; }

        MvxCommand<AccountViewModel> DeleteAccountCommand { get; }
    }
}