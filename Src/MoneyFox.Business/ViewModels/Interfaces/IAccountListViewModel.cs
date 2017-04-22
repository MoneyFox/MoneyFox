using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IAccountListViewModel
    {
        ObservableCollection<AccountViewModel> IncludedAccounts { get; set; }

        ObservableCollection<AccountViewModel> ExcludedAccounts { get; set; }

        bool IsAllAccountsEmpty { get; }

        bool IsExcludedAccountsEmpty { get; }

        IBalanceViewModel BalanceViewModel { get; }

        IViewActionViewModel ViewActionViewModel { get; }

        MvxCommand LoadedCommand { get; }

        MvxCommand<AccountViewModel> EditAccountCommand { get; }

        MvxCommand<AccountViewModel> DeleteAccountCommand { get; }
    }
}