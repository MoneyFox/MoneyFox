using System;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class AccountListViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly IEndOfMonthManager endOfMonthManager;
        private readonly IPaymentRepository paymentRepository;
        private readonly ISettingsManager settingsManager;
        private ObservableCollection<AccountViewModel> allAccounts;

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
            ViewActionViewModel = new ViewActionViewModel();
        }

        #region Properties

        public IBalanceViewModel BalanceViewModel { get; }

        public IViewActionViewModel ViewActionViewModel { get; }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     All existing accounts.
        /// </summary>
        public ObservableCollection<AccountViewModel> AllAccounts
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

        #endregion

        #region Commands

        /// <summary>
        ///     Prepares the AccountViewModel list
        /// </summary>
        public MvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Open the payment overview for this AccountViewModel.
        /// </summary>
        public MvxCommand<AccountViewModel> OpenOverviewCommand => new MvxCommand<AccountViewModel>(GoToPaymentOverView);

        /// <summary>
        ///     Edit the selected AccountViewModel
        /// </summary>
        public MvxCommand<AccountViewModel> EditAccountCommand => new MvxCommand<AccountViewModel>(EditAccount);

        /// <summary>
        ///     Deletes the selected AccountViewModel
        /// </summary>
        public MvxCommand<AccountViewModel> DeleteAccountCommand => new MvxCommand<AccountViewModel>(Delete);

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public MvxCommand GoToAddAccountCommand => new MvxCommand(GoToAddAccount);

        #endregion
        
        private void EditAccount(AccountViewModel accountViewModel)
        {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = true, selectedAccountId = accountViewModel.Id});
        }

        private void Loaded()
        {
            AllAccounts = new ObservableCollection<AccountViewModel>(accountRepository.GetList());
            BalanceViewModel.UpdateBalanceCommand.Execute();
            endOfMonthManager.CheckEndOfMonthBalanceForAccounts(AllAccounts);
        }

        private void GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if (accountViewModel == null)
            {
                return;
            }

            ShowViewModel<PaymentListViewModel>(new {id = accountViewModel.Id});
        }

        private async void Delete(AccountViewModel accountToDelete)
        {
            if (accountToDelete == null)
            {
                return;
            }

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                var paymentsToDelete = paymentRepository.GetList(p => p.ChargedAccountId == accountToDelete.Id);

                foreach (var payment in paymentsToDelete.ToList())
                {
                    paymentRepository.Delete(payment);
                }
                if (accountRepository.Delete(accountToDelete))
                {
                    settingsManager.LastDatabaseUpdate = DateTime.Now;
                }
            }
            BalanceViewModel.UpdateBalanceCommand.Execute();

            if (AllAccounts.Contains(accountToDelete))
            {
                AllAccounts.Remove(accountToDelete);
            }

            // refresh view when an AccountViewModel is deleted allowing buttons to update 
            // TODO probably a better solution
            ShowViewModel<MainViewModel>();
        }

        private void GoToAddAccount()
        {
            ShowViewModel<ModifyAccountViewModel>(new {isEdit = false, selectedAccountId = 0});
        }
    }
}