using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Uwp.ViewModels.Interfaces;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeAccountListViewModel : IAccountListViewModel
    {
        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts { get; } =
            new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>
            {
                new AlphaGroupListGroupCollection<AccountViewModel>("Included")
                {
                    new AccountViewModel {Name = "Income", CurrentBalance = 1234}
                },
                new AlphaGroupListGroupCollection<AccountViewModel>("Excluded")
                {
                    new AccountViewModel {Name = "Savings", CurrentBalance = 4325}
                }
            };

        public bool HasNoAccounts { get; }
        public IBalanceViewModel BalanceViewModel { get; } = new DesignTimeBalanceViewViewModel();
        public IAccountListViewActionViewModel ViewActionViewModel { get; }
        public AsyncCommand LoadDataCommand { get; }
        public RelayCommand<AccountViewModel> OpenOverviewCommand { get; }
        public RelayCommand<AccountViewModel> EditAccountCommand { get; }
        public AsyncCommand<AccountViewModel> DeleteAccountCommand { get; }
        public RelayCommand GoToAddAccountCommand { get; }
    }
}
