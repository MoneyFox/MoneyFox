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
    /// <inheritdoc />
    public class AccountListViewModel : MvxViewModel, IAccountListViewModel
    {
        private readonly IAccountService accountService;
        private readonly IBalanceCalculationManager balanceCalculationManager;
        private readonly ISettingsManager settingsManager;
        private readonly IModifyDialogService modifyDialogService;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;

        private ObservableCollection<AccountViewModel> includedAccounts;
        private ObservableCollection<AccountViewModel> excludedAccounts;
        
        /// <summary>
        ///     Constructor
        /// </summary>
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

        /// <inheritdoc />
        public async Task ShowMenu()
        {
            await navigationService.Navigate<MenuViewModel>();
        }

        #region Properties

        /// <inheritdoc />
        public IBalanceViewModel BalanceViewModel { get; }

        /// <inheritdoc />
        public IViewActionViewModel ViewActionViewModel { get; }

        /// <inheritdoc />
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <inheritdoc />
        public ObservableCollection<AccountViewModel> IncludedAccounts
        {
            get => includedAccounts;
            set
            {
                if (includedAccounts == value) return;
                includedAccounts = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsAllAccountsEmpty));
            }
        }

        /// <inheritdoc />
        public ObservableCollection<AccountViewModel> ExcludedAccounts
        {
            get => excludedAccounts;
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

        /// <inheritdoc />
        public bool IsAllAccountsEmpty => !(IncludedAccounts.Any() || ExcludedAccounts.Any());

        /// <inheritdoc />
        public bool IsExcludedAccountsEmpty => !ExcludedAccounts?.Any() ?? true;

        #endregion

        #region Commands

        /// <inheritdoc />
        public MvxAsyncCommand LoadedCommand => new MvxAsyncCommand(Loaded);

        /// <inheritdoc />
        public MvxAsyncCommand<AccountViewModel> OpenOverviewCommand =>
            new MvxAsyncCommand<AccountViewModel>(GoToPaymentOverView);

        /// <inheritdoc />
        public MvxAsyncCommand<AccountViewModel> EditAccountCommand => new MvxAsyncCommand<AccountViewModel>(EditAccount);

        /// <inheritdoc />
        public MvxAsyncCommand<AccountViewModel> DeleteAccountCommand => new MvxAsyncCommand<AccountViewModel>(Delete);

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddAccountCommand => new MvxAsyncCommand(GoToAddAccount);

        /// <inheritdoc />
        public MvxAsyncCommand<AccountViewModel> OpenContextMenuCommand => new MvxAsyncCommand<AccountViewModel>(OpenContextMenu);

        #endregion

        private async Task EditAccount(AccountViewModel accountViewModel)
        {
            await navigationService.Navigate<ModifyAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter(accountViewModel.Id));
        }

        private async Task Loaded()
        {
            try
            {
                var includedAccountList = await accountService.GetNotExcludedAccounts();
                var excludedAccountList = await accountService.GetExcludedAccounts();

                IncludedAccounts =
                    new ObservableCollection<AccountViewModel>(includedAccountList.Select(x => new AccountViewModel(x)));
                ExcludedAccounts =
                    new ObservableCollection<AccountViewModel>(excludedAccountList.Select(x => new AccountViewModel(x)));

                //await BalanceViewModel.UpdateBalanceCommand.ExecuteAsync();
            }
            catch(Exception ex)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, ex.ToString());
            }
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