using System;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MoneyFox.Business.Manager;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Service.DataServices;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Business.ViewModels
{
    public class AccountListViewModel : BaseViewModel, IAccountListViewModel
    {
        private readonly IAccountService accountService;
        private readonly IBalanceCalculationManager balanceCalculationManager;
        private readonly IPaymentManager paymentManager;
        private readonly ISettingsManager settingsManager;
        private readonly IModifyDialogService modifyDialogService;
        private readonly IDialogService dialogService;

        private ObservableCollection<AccountViewModel> includedAccounts;
        private ObservableCollection<AccountViewModel> excludedAccounts;

        public AccountListViewModel(IAccountService accountService,
                                    IBalanceCalculationManager balanceCalculationManager,
                                    IPaymentManager paymentManager,
                                    ISettingsManager settingsManager, 
                                    IModifyDialogService modifyDialogService,
                                    IDialogService dialogService)
        {
            this.accountService = accountService;
            this.balanceCalculationManager = balanceCalculationManager;
            this.paymentManager = paymentManager;
            this.settingsManager = settingsManager;
            this.modifyDialogService = modifyDialogService;
            this.dialogService = dialogService;

            BalanceViewModel = new BalanceViewModel(balanceCalculationManager);
            //TODO
            //ViewActionViewModel = new AccountListViewActionViewModel(accountService);

            IncludedAccounts = new MvxObservableCollection<AccountViewModel>();
            ExcludedAccounts = new MvxObservableCollection<AccountViewModel>();
        }

        /// <summary>
		/// 	Used on Ios
		/// </summary>
		public void ShowMenu()
		{
			ShowViewModel<MenuViewModel>();
		}

		#region Properties

		public IBalanceViewModel BalanceViewModel { get; }

        public IViewActionViewModel ViewActionViewModel { get; }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     All existing accounts who are included to the balance calculation.
        /// </summary>
        public ObservableCollection<AccountViewModel> IncludedAccounts
        {
            get { return includedAccounts; }
            set
            {
                if(includedAccounts == value) return;
                includedAccounts = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsAllAccountsEmpty));
            }
        }

        /// <summary>
        ///     All existing accounts who are exluded from the balance calculation.
        /// </summary>
        public ObservableCollection<AccountViewModel> ExcludedAccounts
        {
            get { return excludedAccounts; }
            set
            {
                if(excludedAccounts == value) return;
                excludedAccounts = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsAllAccountsEmpty));
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsExcludedAccountsEmpty));
            }
        }

        /// <summary>
        ///     Returns if the IncludedAccounts Collection is emtpy or not.
        /// </summary>
        public bool IsAllAccountsEmpty => !(IncludedAccounts.Any() || ExcludedAccounts.Any());

        /// <summary>
        ///     Returns if the ExcludedAccounts Collection is emtpy or not.
        /// </summary>accoutn
        public bool IsExcludedAccountsEmpty => !ExcludedAccounts?.Any() ?? true;

        #endregion

        #region Commands

        /// <summary>
        ///     Prepares the Account list
        /// </summary>
        public MvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Open the payment overview for this Account.
        /// </summary>
        public MvxCommand<Account> OpenOverviewCommand => new MvxCommand<Account>(GoToPaymentOverView);

        /// <summary>
        ///     Edit the selected Account
        /// </summary>
        public MvxCommand<Account> EditAccountCommand => new MvxCommand<Account>(EditAccount);

        /// <summary>
        ///     Deletes the selected Account
        /// </summary>
        public MvxCommand<Account> DeleteAccountCommand => new MvxCommand<Account>(Delete);

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public MvxCommand GoToAddAccountCommand => new MvxCommand(GoToAddAccount);

        /// <summary>
        ///     Opens the Context Menu for a list or a recycler view
        /// </summary>
        public MvxCommand<Account> OpenContextMenuCommand => new MvxCommand<Account>(OpenContextMenu);

        #endregion

        private void EditAccount(Account accountViewModel)
        {
            //TODO
            //ShowViewModel<ModifyAccountViewModel>(new { accountId = accountViewModel.Id});
        }

        private async void Loaded()
        {
            var includedAccountList = await accountService.GetNotExcludedAccounts();
            var excludedAccountList = await accountService.GetNotExcludedAccounts();

            IncludedAccounts = new ObservableCollection<AccountViewModel>(includedAccountList.Select(x => new AccountViewModel(x)));
            ExcludedAccounts = new ObservableCollection<AccountViewModel>(excludedAccountList.Select(x => new AccountViewModel(x)));

            BalanceViewModel.UpdateBalanceCommand.Execute();
            await balanceCalculationManager.GetTotalEndOfMonthBalance();
        }

        private void GoToPaymentOverView(Account accountViewModel)
        {
            if (accountViewModel == null)
            {
                return;
            }

            // TODO
           // ShowViewModel<PaymentListViewModel>(new {id = accountViewModel.Id});
        }

        private async void Delete(Account accountToDelete)
        {
            if (accountToDelete == null)
            {
                return;
            }

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                paymentManager.DeleteAssociatedPaymentsFromDatabase(accountToDelete);

                if (accountRepository.Delete(accountToDelete))
                {
                    if (IncludedAccounts.Contains(accountToDelete))
                    {
                        IncludedAccounts.Remove(accountToDelete);
                        // ReSharper disable once ExplicitCallerInfoArgument
                        RaisePropertyChanged(nameof(IncludedAccounts));
                    }
                    if (ExcludedAccounts.Contains(accountToDelete))
                    {
                        ExcludedAccounts.Remove(accountToDelete);
                        // ReSharper disable once ExplicitCallerInfoArgument
                        RaisePropertyChanged(nameof(ExcludedAccounts));
                    }
                    settingsManager.LastDatabaseUpdate = DateTime.Now;
                }
                else
                {
                    await dialogService
                        .ShowConfirmMessage(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
                }
            }
            BalanceViewModel.UpdateBalanceCommand.Execute();
        }

        private void GoToAddAccount()
        {
            ShowViewModel<ModifyAccountViewModel>(new {selectedAccountId = 0});
        }

        private async void OpenContextMenu(Account account)
        {
            var result = await modifyDialogService.ShowEditSelectionDialog();

            switch (result)
            {
                case ModifyOperation.Edit:
                    EditAccountCommand.Execute(account);
                    break;

                case ModifyOperation.Delete:
                    DeleteAccountCommand.Execute(account);
                    break;
            }
        }
    }
}