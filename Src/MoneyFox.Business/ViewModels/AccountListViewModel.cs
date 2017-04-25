using System;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Business.Manager;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class AccountListViewModel : BaseViewModel, IAccountListViewModel
    {
        private readonly IAccountService accountService;
        private readonly IBalanceCalculationManager balanceCalculationManager;
        private readonly ISettingsManager settingsManager;
        private readonly IModifyDialogService modifyDialogService;
        private readonly IDialogService dialogService;

        private ObservableCollection<AccountViewModel> includedAccounts;
        private ObservableCollection<AccountViewModel> excludedAccounts;

        public AccountListViewModel(IAccountService accountService,
                                    IBalanceCalculationManager balanceCalculationManager,
                                    ISettingsManager settingsManager,
                                    IModifyDialogService modifyDialogService,
                                    IDialogService dialogService)
        {
            this.accountService = accountService;
            this.balanceCalculationManager = balanceCalculationManager;
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
        ///     Used on Ios
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
                if (includedAccounts == value) return;
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
                if (excludedAccounts == value) return;
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
        /// </summary>
        /// accoutn
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
        public MvxCommand<AccountViewModel> OpenOverviewCommand =>
            new MvxCommand<AccountViewModel>(GoToPaymentOverView);

        /// <summary>
        ///     Edit the selected Account
        /// </summary>
        public MvxCommand<AccountViewModel> EditAccountCommand => new MvxCommand<AccountViewModel>(EditAccount);

        /// <summary>
        ///     Deletes the selected Account
        /// </summary>
        public MvxCommand<AccountViewModel> DeleteAccountCommand => new MvxCommand<AccountViewModel>(Delete);

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public MvxCommand GoToAddAccountCommand => new MvxCommand(GoToAddAccount);

        /// <summary>
        ///     Opens the Context Menu for a list or a recycler view
        /// </summary>
        public MvxCommand<AccountViewModel> OpenContextMenuCommand => new MvxCommand<AccountViewModel>(OpenContextMenu);

        #endregion

        private void EditAccount(AccountViewModel accountViewModel)
        {
            //TODO
            //ShowViewModel<ModifyAccountViewModel>(new { accountId = accountViewModel.Id});
        }

        private async void Loaded()
        {
            var includedAccountList = await accountService.GetNotExcludedAccounts();
            var excludedAccountList = await accountService.GetNotExcludedAccounts();

            IncludedAccounts =
                new ObservableCollection<AccountViewModel>(includedAccountList.Select(x => new AccountViewModel(x)));
            ExcludedAccounts =
                new ObservableCollection<AccountViewModel>(excludedAccountList.Select(x => new AccountViewModel(x)));

            BalanceViewModel.UpdateBalanceCommand.Execute();
            await balanceCalculationManager.GetTotalEndOfMonthBalance();
        }

        private void GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if (accountViewModel == null)
            {
            }

            // TODO
            // ShowViewModel<PaymentListViewModel>(new {id = accountViewModel.Id});
        }

        private async void Delete(AccountViewModel accountToDelete)
        {
            if (accountToDelete == null)
            {
                return;
            }

            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await accountService.DeleteAccount(accountToDelete.Account);

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
            BalanceViewModel.UpdateBalanceCommand.Execute();
        }

        private void GoToAddAccount()
        {
            //TODO
            //ShowViewModel<ModifyAccountViewModel>(new {selectedAccountId = 0});
        }

        private async void OpenContextMenu(AccountViewModel account)
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