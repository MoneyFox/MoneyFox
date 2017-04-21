using System.Collections.ObjectModel;
using MoneyFox.Service.Pocos;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    public interface IAccountListViewModel
    {
        ObservableCollection<Account> IncludedAccounts { get; set; }

        ObservableCollection<Account> ExcludedAccounts { get; set; }

        bool IsAllAccountsEmpty { get; }

        bool IsExcludedAccountsEmpty { get; }

        IBalanceViewModel BalanceViewModel { get; }

        IViewActionViewModel ViewActionViewModel { get; }

        MvxCommand LoadedCommand { get; }

        MvxCommand<Account> EditAccountCommand { get; }

        MvxCommand<Account> DeleteAccountCommand { get; }
    }
}