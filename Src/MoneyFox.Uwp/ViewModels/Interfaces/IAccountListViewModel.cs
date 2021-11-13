using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.Commands;
using MoneyFox.Uwp.Groups;
using MoneyFox.Uwp.ViewModels.Accounts;
using System.Collections.ObjectModel;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Interfaces
{
    /// <summary>
    /// Representation of the AccountListView.
    /// </summary>
    public interface IAccountListViewModel
    {
        /// <summary>
        /// All existing accounts
        /// </summary>
        ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts { get; }

        /// <summary>
        /// Indicates if there are accounts to display.
        /// </summary>
        bool HasNoAccounts { get; }

        /// <summary>
        /// View Model for the balance view integrated in the account list view
        /// </summary>
        IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        /// View Model for the actions associated with the account list.
        /// </summary>
        IAccountListViewActionViewModel ViewActionViewModel { get; }

        /// <summary>
        /// Loads the data
        /// </summary>
        AsyncCommand LoadDataCommand { get; }

        /// <summary>
        /// Open the payment overview for this Account.
        /// </summary>
        RelayCommand<AccountViewModel> OpenOverviewCommand { get; }

        /// <summary>
        /// Edit the selected Account
        /// </summary>
        RelayCommand<AccountViewModel> EditAccountCommand { get; }

        /// <summary>
        /// Deletes the selected Account
        /// </summary>
        AsyncCommand<AccountViewModel> DeleteAccountCommand { get; }
    }
}