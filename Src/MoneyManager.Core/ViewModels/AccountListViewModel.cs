using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class AccountListViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;
        private readonly TransactionListViewModel transactionListViewModel;
        private readonly BalanceViewModel balanceViewModel;
        private readonly ModifyAccountViewModel modifyAccountViewModel;

        public AccountListViewModel(IRepository<Account> accountRepository,
            TransactionListViewModel transactionListViewModel,
            BalanceViewModel balanceViewModel, ModifyAccountViewModel modifyAccountViewModel)
        {
            this.accountRepository = accountRepository;
            this.transactionListViewModel = transactionListViewModel;
            this.balanceViewModel = balanceViewModel;
            this.modifyAccountViewModel = modifyAccountViewModel;
        }

        /// <summary>
        ///     All existing accounts.
        /// </summary>
        public ObservableCollection<Account> AllAccounts
        {
            get { return accountRepository.Data; }
            set { accountRepository.Data = value; }
        }

        /// <summary>
        ///     Open the transaction overview for this account.
        /// </summary>
        public MvxCommand<Account> OpenOverviewCommand => new MvxCommand<Account>(Delete);

        /// <summary>
        ///     Edit the selected account
        /// </summary>
        public MvxCommand<Account> EditAccountCommand => new MvxCommand<Account>(EditAccount);

        /// <summary>
        ///     Deletes the selected account
        /// </summary>
        public MvxCommand<Account> DeleteAccountCommand=> new MvxCommand<Account>(GoToTransactionOverView);

        private void EditAccount(Account account)
        {
            modifyAccountViewModel.IsEdit = true;
            modifyAccountViewModel.SelectedAccount = account;

            ShowViewModel<ModifyAccountViewModel>();
        }

        private void GoToTransactionOverView(Account selectedAccount)
        {
            if (selectedAccount == null)
            {
                return;
            }

            accountRepository.Selected = selectedAccount;
            transactionListViewModel.LoadedCommand.Execute(accountRepository.Selected);

            accountRepository.Selected = null;

            ShowViewModel<TransactionListViewModel>();
        }

        private void Delete(Account item)
        {
            balanceViewModel.UpdateBalance();
            accountRepository.Delete(item);
        }
    }
}