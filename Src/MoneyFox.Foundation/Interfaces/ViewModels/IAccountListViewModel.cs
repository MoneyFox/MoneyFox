using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Foundation.Interfaces.ViewModels
{
    public interface IAccountListViewModel
    {
        ObservableCollection<AccountViewModel> IncludedAccounts { get; set; }

        IBalanceViewModel BalanceViewModel { get; }

        IViewActionViewModel ViewActionViewModel { get; }

        MvxCommand LoadedCommand { get; }

        MvxCommand<AccountViewModel> EditAccountCommand { get; }

        MvxCommand<AccountViewModel> DeleteAccountCommand { get; }
    }
}