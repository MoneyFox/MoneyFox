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

        public AccountListViewModel(IRepository<Account> accountRepository)
        {
            this.accountRepository = accountRepository;
            OpenOverviewCommand = new MvxCommand<Account>(GoToTransactionOverView);
            DeleteAccountCommand = new MvxCommand<Account>(Delete);
        }

        /// <summary>
        /// All existing accounts.
        /// </summary>
        public ObservableCollection<Account> AllAccounts
        {
            get { return accountRepository.Data; }
            set
            {
                accountRepository.Data = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Open the transaction overview for this account.
        /// </summary>
        public MvxCommand<Account> OpenOverviewCommand { get; set; }

        /// <summary>
        /// Deletes the selected account
        /// </summary>
        public MvxCommand<Account> DeleteAccountCommand { get; set; }
        
        private void GoToTransactionOverView(Account selectedAccount)
        {
            accountRepository.Selected = selectedAccount;
            ShowViewModel<TransactionListViewModel>();
        }

        private void Delete(Account item)
        {
            accountRepository.Delete(item);
        }
    }
}