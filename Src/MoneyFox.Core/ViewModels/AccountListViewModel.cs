using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class AccountListViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;

        public AccountListViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IDialogService dialogService)
        {
            this.accountRepository = accountRepository;
            this.dialogService = dialogService;

            BalanceViewModel = new BalanceViewModel(accountRepository, paymentRepository);
        }

        public IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     All existing accounts.
        /// </summary>
        public ObservableCollection<Account> AllAccounts
        {
            get { return accountRepository.Data; }
            set { accountRepository.Data = value; }
        }

        /// <summary>
        ///     Prepares the account list
        /// </summary>
        public RelayCommand LoadedCommand => new RelayCommand(Loaded);

        /// <summary>
        ///     Open the payment overview for this account.
        /// </summary>
        public RelayCommand<Account> OpenOverviewCommand => new RelayCommand<Account>(GoToPaymentOverView);

        /// <summary>
        ///     Edit the selected account
        /// </summary>
        public RelayCommand<Account> EditAccountCommand => new RelayCommand<Account>(EditAccount);

        /// <summary>
        ///     Deletes the selected account
        /// </summary>
        public RelayCommand<Account> DeleteAccountCommand => new RelayCommand<Account>(Delete);

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public RelayCommand GoToAddAccountCommand => new RelayCommand(GoToAddAccount);

        private void EditAccount(Account account)
        {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = true, selectedAccountId = account.Id});
        }

        private void Loaded()
        {
            BalanceViewModel.UpdateBalanceCommand.Execute();
        }

        private void GoToPaymentOverView(Account account)
        {
            if (account == null)
            {
                return;
            }

            accountRepository.Selected = account;
            ShowViewModel<PaymentListViewModel>();
        }

        private async void Delete(Account item)
        {
            if (item == null)
            {
                return;
            }

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                accountRepository.Delete(item);
            }
        }

        private void GoToAddAccount()
        {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = true, selectedAccountId = 0});
        }
    }
}