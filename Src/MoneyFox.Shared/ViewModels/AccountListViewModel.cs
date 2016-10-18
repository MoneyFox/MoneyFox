using System;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Interfaces.ViewModels;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Shared.ViewModels
{
    public class AccountListViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly IEndOfMonthManager endOfMonthManager;
        private readonly IPaymentRepository paymentRepository;
        private readonly ISettingsManager settingsManager;
        private ObservableCollection<Account> allAccounts;

        public AccountListViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IDialogService dialogService, 
            IEndOfMonthManager endOfMonthManager,
            ISettingsManager settingsManager)
        {
            this.dialogService = dialogService;
            this.accountRepository = accountRepository;
            this.paymentRepository = paymentRepository;
            this.endOfMonthManager = endOfMonthManager;
            this.settingsManager = settingsManager;

            BalanceViewModel = new BalanceViewModel(accountRepository, endOfMonthManager);
        }

        public IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     All existing accounts.
        /// </summary>
        public ObservableCollection<Account> AllAccounts
        {
            get { return allAccounts; }
            set
            {
                if(allAccounts == value) return;
                allAccounts = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsAllAccountsEmpty));
            }
        }

        /// <summary>
        ///     Returns if the ChargedAccounts Collection is emtpy or not.
        /// </summary>
        public bool IsAllAccountsEmpty => !AllAccounts?.Any() ?? true;

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

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        private void EditAccount(Account account)
        {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = true, selectedAccountId = account.Id});
        }

        private void Loaded()
        {
            AllAccounts = new ObservableCollection<Account>(accountRepository.GetList());
            BalanceViewModel.UpdateBalanceCommand.Execute();
            endOfMonthManager.CheckEndOfMonthBalanceForAccounts(AllAccounts);
        }

        private void GoToPaymentOverView(Account account)
        {
            if (account == null)
            {
                return;
            }

            ShowViewModel<PaymentListViewModel>(new {id = account.Id});
        }

        private async void Delete(Account item)
        {
            if (item == null)
            {
                return;
            }

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                var paymentsToDelete = paymentRepository.GetList(p => p.ChargedAccountId == item.Id);

                foreach (var payment in paymentsToDelete.ToList())
                {
                    paymentRepository.Delete(payment);
                }
                if (accountRepository.Delete(item))
                {
                    settingsManager.LastDatabaseUpdate = DateTime.Now;
                }
            }
            BalanceViewModel.UpdateBalanceCommand.Execute();

            // refresh view when an account is deleted allowing buttons to update 
            // TODO probably a better solution
            ShowViewModel<MainViewModel>();
        }

        private void GoToAddAccount()
        {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = true, selectedAccountId = 0});
        }
    }
}