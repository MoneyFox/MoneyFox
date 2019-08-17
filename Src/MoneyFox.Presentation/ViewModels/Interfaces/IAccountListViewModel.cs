using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Groups;

namespace MoneyFox.Presentation.ViewModels.Interfaces
{
    /// <summary>
    ///     Representation of the AccountListView.
    /// </summary>
    public interface IAccountListViewModel : IBaseViewModel
    {        
        /// <summary>
        ///     All existing accounts
        /// </summary>
        ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts { get; }

        /// <summary>
        ///     Indicates if there are accounts to display.
        /// </summary>
        bool HasNoAccounts { get; }

        /// <summary>
        ///     View Model for the balance view integrated in the account list view
        /// </summary>
        IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     View Mdoel for the actions associated with the account list.
        /// </summary>
        IAccountListViewActionViewModel ViewActionViewModel { get; }

        /// <summary>
        ///     Loads the data
        /// </summary>
        AsyncCommand LoadDataCommand { get; }

        /// <summary>
        ///     Open the payment overview for this Account.
        /// </summary>
        RelayCommand<AccountViewModel> OpenOverviewCommand { get; }

        /// <summary>
        ///     Edit the selected Account
        /// </summary>
        RelayCommand<AccountViewModel> EditAccountCommand { get; }

        /// <summary>
        ///     Deletes the selected Account
        /// </summary>
        AsyncCommand<AccountViewModel> DeleteAccountCommand { get; }

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        RelayCommand GoToAddAccountCommand { get; }
    }
}
