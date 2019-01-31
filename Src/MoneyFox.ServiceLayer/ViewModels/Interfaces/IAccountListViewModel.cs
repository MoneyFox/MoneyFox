using System.Collections.ObjectModel;
using MoneyFox.Foundation.Groups;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.Interfaces
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
        ///     Open the payment overview for this Account.
        /// </summary>
        MvxAsyncCommand<AccountViewModel> OpenOverviewCommand { get; }

        /// <summary>
        ///     Edit the selected Account
        /// </summary>
        MvxAsyncCommand<AccountViewModel> EditAccountCommand { get; }

        /// <summary>
        ///     Deletes the selected Account
        /// </summary>
        MvxAsyncCommand<AccountViewModel> DeleteAccountCommand { get; }       
        
        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        MvxAsyncCommand GoToAddAccountCommand { get; }
    }
}