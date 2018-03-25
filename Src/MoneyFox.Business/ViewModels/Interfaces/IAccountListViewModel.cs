using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels.Interfaces
{    
    /// <summary>
    ///     Representation of the AccountListView.
    /// </summary>
    public interface IAccountListViewModel
    {        
        /// <summary>
        ///     All existing accounts
        /// </summary>
        MvxObservableCollection<AlphaGroupListGroup<AccountViewModel>> Accounts { get; }

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
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        IMvxLanguageBinder TextSource { get; }
        
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