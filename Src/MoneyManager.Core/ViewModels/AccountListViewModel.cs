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
        
        public Account SelectedAccount { get; set; }

        /// <summary>
        ///     Open the transaction overview for this account.
        /// </summary>
        public MvxCommand OpenOverviewCommand => new MvxCommand(GoToTransactionOverView);

        /// <summary>
        ///     Edit the selected account
        /// </summary>
        public MvxCommand<Account> EditAccountCommand => new MvxCommand<Account>(EditAccount);

        /// <summary>
        ///     Deletes the selected account
        /// </summary>
        public MvxCommand<Account> DeleteAccountCommand=> new MvxCommand<Account>(Delete);

        private void EditAccount(Account account)
        {
            modifyAccountViewModel.IsEdit = true;
            modifyAccountViewModel.SelectedAccount = account;

            ShowViewModel<ModifyAccountViewModel>();
        }

        private void GoToTransactionOverView()
        {
            if (SelectedAccount == null)
            {
                return;
            }

            accountRepository.Selected = SelectedAccount;
            transactionListViewModel.LoadedCommand.Execute();

            SelectedAccount = null;

            ShowViewModel<TransactionListViewModel>();
        }

        private void Delete(Account item)
        {
            balanceViewModel.UpdateBalance();
            accountRepository.Delete(item);
        }
    }
}