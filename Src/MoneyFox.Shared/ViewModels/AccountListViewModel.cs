using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.ViewModels;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using PropertyChanged;
using MoneyFox.Shared.Helpers;
using System;

namespace MoneyFox.Shared.ViewModels {
    [ImplementPropertyChanged]
    public class AccountListViewModel : BaseViewModel {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly IPaymentRepository paymentRepository;

        public AccountListViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IDialogService dialogService) {
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
            this.dialogService = dialogService;

            BalanceViewModel = new BalanceViewModel(accountRepository, paymentRepository);
        }

        public IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     All existing accounts.
        /// </summary>
        public ObservableCollection<Account> AllAccounts {
            get { return accountRepository.Data; }
            set { accountRepository.Data = value; }
        }

        /// <summary>
        ///     Returns if the AllAccounts Collection is emtpy or not.
        /// </summary>
        public bool IsAllAccountsEmpty => !AllAccounts.Any();

        /// <summary>
        ///     Prepares the account list
        /// </summary>
        public MvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Open the payment overview for this account.
        /// </summary>
        public MvxCommand<Account> OpenOverviewCommand => new MvxCommand<Account>(GoToPaymentOverView);

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

        private void EditAccount(Account account) {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = true, selectedAccountId = account.Id});
        }

        private void Loaded() {
            BalanceViewModel.UpdateBalanceCommand.Execute();
        }

        private void GoToPaymentOverView(Account account) {
            if (account == null) {
                return;
            }

            ShowViewModel<PaymentListViewModel>(new { id = account.Id});
        }

        private async void Delete(Account item) {
            if (item == null) {
                return;
            }

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                var paymentsToDelete = paymentRepository.Data.Where(p => p.ChargedAccountId == item.Id);

                if (null != paymentsToDelete)
                {
                    foreach (var payment in paymentsToDelete.ToList())
                    {
                        paymentRepository.Delete(payment);
                    }
                }
                if(accountRepository.Delete(item))
                    SettingsHelper.LastDatabaseUpdate = DateTime.Now;
            }
            BalanceViewModel.UpdateBalanceCommand.Execute();
        }

        private void GoToAddAccount() {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = true, selectedAccountId = 0});
        }
    }
}