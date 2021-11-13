using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.Groups;
using MoneyFox.Uwp.ViewModels.Accounts;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    [SuppressMessage(
        "Major Code Smell",
        "S109:Magic numbers should not be used",
        Justification = "Not needed in design time")]
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
                    new AccountViewModel {Name = "Income", CurrentBalance = 1234}
                },
                new AlphaGroupListGroupCollection<AccountViewModel>("Excluded")
                {
                    new AccountViewModel {Name = "Savings", CurrentBalance = 4325}
                }
            };

        public bool HasNoAccounts { get; }

        public IBalanceViewModel BalanceViewModel { get; } = new DesignTimeBalanceViewViewModel();

        public IAccountListViewActionViewModel ViewActionViewModel { get; } = null!;

        public AsyncRelayCommand LoadDataCommand { get; } = null!;

        public RelayCommand<AccountViewModel> OpenOverviewCommand { get; } = null!;

        public RelayCommand<AccountViewModel> EditAccountCommand { get; } = null!;

        public AsyncRelayCommand<AccountViewModel> DeleteAccountCommand { get; } = null!;

        public RelayCommand GoToAddAccountCommand { get; } = null!;
    }
}