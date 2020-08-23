using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : IAccountListViewModel
    {
        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts
        {
            get;
        } =
            new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>
        {
            new AlphaGroupListGroupCollection<AccountViewModel>("Included")
            {
                new AccountViewModel { Name = "Income", CurrentBalance = 1234 }
            },
            new AlphaGroupListGroupCollection<AccountViewModel>("Excluded")
            {
                new AccountViewModel { Name = "Savings", CurrentBalance = 4325 }
            }
        };

        public bool HasNoAccounts { get; }

        public IBalanceViewModel BalanceViewModel { get; } = new DesignTimeBalanceViewViewModel();

        public IAccountListViewActionViewModel ViewActionViewModel { get; }

        public AsyncCommand LoadDataCommand { get; } = null!;

        public RelayCommand<AccountViewModel> OpenOverviewCommand { get; } = null!;

        public RelayCommand<AccountViewModel> EditAccountCommand { get; } = null!;

        public AsyncCommand<AccountViewModel> DeleteAccountCommand { get; } = null!;

        public RelayCommand GoToAddAccountCommand { get; } = null!;
    }
}
