using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the AccountListView.
    /// </summary>
    public class AccountListViewModel : MvxViewModel, IAccountListViewModel
    {
        private readonly IAccountService accountService;
        private readonly IBalanceCalculationManager balanceCalculationManager;
        private readonly ISettingsManager settingsManager;
        private readonly IModifyDialogService modifyDialogService;
        private readonly IDialogService dialogService;
        protected readonly IMvxNavigationService navigationService;

        private ObservableCollection<AccountViewModel> includedAccounts;
        private ObservableCollection<AccountViewModel> excludedAccounts;

        public AccountListViewModel(IAccountService accountService,
                                    IBalanceCalculationManager balanceCalculationManager,
                                    ISettingsManager settingsManager,
                                    IModifyDialogService modifyDialogService,
                                    IDialogService dialogService, 
                                    IMvxNavigationService navigationService)
        {
            this.accountService = accountService;
            this.balanceCalculationManager = balanceCalculationManager;
            this.settingsManager = settingsManager;
            this.modifyDialogService = modifyDialogService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            BalanceViewModel = new BalanceViewModel(balanceCalculationManager);
            ViewActionViewModel = new AccountListViewActionViewModel(accountService, navigationService);

            IncludedAccounts = new MvxObservableCollection<AccountViewModel>();
            ExcludedAccounts = new MvxObservableCollection<AccountViewModel>();
        }

        /// <summary>
        ///     Used on Ios
        /// </summary>
        public async Task ShowMenu()
        {
            await navigationService.Navigate<MenuViewModel>();
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
        public MvxAsyncCommand<AccountViewModel> OpenOverviewCommand =>
            new MvxAsyncCommand<AccountViewModel>(GoToPaymentOverView);

        /// <summary>
        ///     Edit the selected Account
        /// </summary>
        public MvxAsyncCommand<AccountViewModel> EditAccountCommand => new MvxAsyncCommand<AccountViewModel>(EditAccount);

        /// <summary>
        ///     Deletes the selected Account
        /// </summary>
        public MvxAsyncCommand<AccountViewModel> DeleteAccountCommand => new MvxAsyncCommand<AccountViewModel>(Delete);

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public MvxAsyncCommand GoToAddAccountCommand => new MvxAsyncCommand(GoToAddAccount);

        /// <summary>
        ///     Opens the Context Menu for a list or a recycler view
        /// </summary>
        public MvxAsyncCommand<AccountViewModel> OpenContextMenuCommand => new MvxAsyncCommand<AccountViewModel>(OpenContextMenu);

        #endregion

        private async Task EditAccount(AccountViewModel accountViewModel)
        {
            await navigationService.Navigate<ModifyAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter(accountViewModel.Id));
        }

        private async void Loaded()
        {
            var includedAccountList = await accountService.GetNotExcludedAccounts();
            var excludedAccountList = await accountService.GetExcludedAccounts();

            IncludedAccounts =
                new ObservableCollection<AccountViewModel>(includedAccountList.Select(x => new AccountViewModel(x)));
            ExcludedAccounts =
                new ObservableCollection<AccountViewModel>(excludedAccountList.Select(x => new AccountViewModel(x)));

            BalanceViewModel.UpdateBalanceCommand.Execute();
            await balanceCalculationManager.GetTotalEndOfMonthBalance();
        }

        private async Task GoToPaymentOverView(AccountViewModel accountViewModel)
        {
            if (accountViewModel == null) return;

            await navigationService.Navigate<PaymentListViewModel, PaymentListParameter>(new PaymentListParameter(accountViewModel.Id));
        }

        private async Task Delete(AccountViewModel accountToDelete)
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

        private async Task GoToAddAccount()
        {
            await navigationService.Navigate<ModifyAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter());
        }

        private async Task OpenContextMenu(AccountViewModel account)
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