namespace MoneyFox.Win.ViewModels.DesignTime;

using Accounts;
using CommunityToolkit.Mvvm.Input;
using Groups;
using Interfaces;
using System.Collections.ObjectModel;

public class DesignTimeAccountListViewModel : IAccountListViewModel
{
    public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts
    {
        get;
    } =
        new()
        {
            new("Included") {new AccountViewModel {Name = "Income", CurrentBalance = 1234}},
            new("Excluded") {new AccountViewModel {Name = "Savings", CurrentBalance = 4325}}
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