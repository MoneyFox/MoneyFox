namespace MoneyFox.Win.ViewModels.DesignTime;

using System.Collections.ObjectModel;
using Accounts;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using Interfaces;

public class DesignTimeAccountListViewModel : IAccountListViewModel
{
    public RelayCommand GoToAddAccountCommand { get; } = null!;

    public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts { get; } = new()
    {
        new("Included") { new() { Name = "Income", CurrentBalance = 1234 } }, new("Excluded") { new() { Name = "Savings", CurrentBalance = 4325 } }
    };

    public bool HasNoAccounts { get; }

    public IBalanceViewModel BalanceViewModel { get; } = new DesignTimeBalanceViewViewModel();

    public IAccountListViewActionViewModel ViewActionViewModel { get; } = null!;

    public AsyncRelayCommand LoadDataCommand { get; } = null!;

    public RelayCommand<AccountViewModel> OpenOverviewCommand { get; } = null!;

    public RelayCommand<AccountViewModel> EditAccountCommand { get; } = null!;

    public AsyncRelayCommand<AccountViewModel> DeleteAccountCommand { get; } = null!;
}
