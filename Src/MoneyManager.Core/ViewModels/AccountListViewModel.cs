using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class AccountListViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;
        private readonly BalanceViewModel balanceViewModel;
        private readonly IDialogService dialogService;
        private readonly ModifyAccountViewModel modifyAccountViewModel;
        private readonly TransactionListViewModel transactionListViewModel;

        public AccountListViewModel(IRepository<Account> accountRepository,
            TransactionListViewModel transactionListViewModel,
            BalanceViewModel balanceViewModel, ModifyAccountViewModel modifyAccountViewModel,
            IDialogService dialogService)
        {
            this.accountRepository = accountRepository;
            this.transactionListViewModel = transactionListViewModel;
            this.balanceViewModel = balanceViewModel;
            this.modifyAccountViewModel = modifyAccountViewModel;
            this.dialogService = dialogService;
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
        public MvxCommand<Account> OpenOverviewCommand => new MvxCommand<Account>(GoToTransactionOverView);

        /// <summary>
        ///     Edit the selected account
        /// </summary>
        public MvxCommand<Account> EditAccountCommand => new MvxCommand<Account>(EditAccount);

        /// <summary>
        ///     Deletes the selected account
        /// </summary>
        public MvxCommand<Account> DeleteAccountCommand => new MvxCommand<Account>(Delete);

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public MvxCommand GoToAddAccountCommand => new MvxCommand(GoToAddAccount);

        private void EditAccount(Account account)
        {
            modifyAccountViewModel.IsEdit = true;
            modifyAccountViewModel.SelectedAccount = account;

            ShowViewModel<ModifyAccountViewModel>();
        }

        private void GoToTransactionOverView(Account account)
        {
            if (account == null)
            {
                return;
            }

            accountRepository.Selected = account;
            transactionListViewModel.LoadedCommand.Execute();

            ShowViewModel<TransactionListViewModel>();
        }

        private async void Delete(Account item)
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                accountRepository.Delete(item);
                balanceViewModel.UpdateBalance();
            }
        }

        private void GoToAddAccount()
        {
            modifyAccountViewModel.IsEdit = false;
            modifyAccountViewModel.SelectedAccount = new Account();

            ShowViewModel<ModifyAccountViewModel>();
        }
    }
}