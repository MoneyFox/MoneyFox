using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        ///     All existing accounts who are included to the balance calculation.
        /// </summary>
        ObservableCollection<AccountViewModel> IncludedAccounts { get; set; }

        /// <summary>
        ///     All existing accounts who are exluded from the balance calculation.
        /// </summary>
        ObservableCollection<AccountViewModel> ExcludedAccounts { get; set; }

        /// <summary>
        ///     Returns if the IncludedAccounts Collection is emtpy or not.
        /// </summary>
        bool IsAllAccountsEmpty { get; }

        /// <summary>
        ///     Returns if the ExcludedAccounts Collection is emtpy or not.
        /// </summary>
        bool IsExcludedAccountsEmpty { get; }

        /// <summary>
        ///     View Model for the balance view integrated in the account list view
        /// </summary>
        IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     View Mdoel for the actions associated with the account list.
        /// </summary>
        IViewActionViewModel ViewActionViewModel { get; }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        IMvxLanguageBinder TextSource { get; }

        /// <summary>
        ///     Prepares the Account list
        /// </summary>
        MvxAsyncCommand LoadedCommand { get; }

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

        /// <summary>
        ///     Opens the Context Menu for a list or a recycler view
        /// </summary>
        MvxAsyncCommand<AccountViewModel> OpenContextMenuCommand { get; }

        /// <summary>
        ///     Used on Ios to load the menus
        /// </summary>
        Task ShowMenu();
    }
}