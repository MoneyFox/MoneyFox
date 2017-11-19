using System;
using System.Threading.Tasks;
using MoneyFox.Business.Messages;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    /// <inheritdoc />
    public class PaymentListViewActionViewModel : MvxViewModel, IPaymentListViewActionViewModel
    {
        private readonly IAccountService accountService;
        private readonly ISettingsManager settingsManager;
        private readonly IDialogService dialogService;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly IMvxNavigationService navigationService;
        private readonly IMvxMessenger messenger;
        private readonly int accountId;
        private bool isClearedFilterActive;
        private bool isRecurringFilterActive;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PaymentListViewActionViewModel(IAccountService accountService,
                                              ISettingsManager settingsManager,
                                              IDialogService dialogService,
                                              IBalanceViewModel balanceViewModel,
                                              IMvxNavigationService navigationService,
                                              IMvxMessenger messenger,
                                              int accountId)
        {
            this.accountService = accountService;
            this.settingsManager = settingsManager;
            this.dialogService = dialogService;
            this.balanceViewModel = balanceViewModel;
            this.navigationService = navigationService;
            this.messenger = messenger;
            this.accountId = accountId;
        }

        #region Commands

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddIncomeCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Income)));

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddExpenseCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Expense)));
        
        /// <inheritdoc />
        public MvxAsyncCommand GoToAddTransferCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Transfer)));

        /// <inheritdoc />
        public MvxAsyncCommand DeleteAccountCommand => new MvxAsyncCommand(DeleteAccount);

        /// <inheritdoc />
        public bool IsClearedFilterActive
        {
            get => isClearedFilterActive;
            set
            {
                if (isClearedFilterActive == value) return;
                isClearedFilterActive = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        /// <inheritdoc />
        public bool IsRecurringFilterActive
        {
            get => isRecurringFilterActive;
            set
            {
                if(isRecurringFilterActive == value) return;
                isRecurringFilterActive = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable => accountService.GetAccountCount().Result > 1;

        /// <summary>
        ///     Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable => accountService.GetAccountCount().Result > 0;

        /// <summary>
        ///     Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable => accountService.GetAccountCount().Result > 0;
        
        #endregion

        private async Task DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                await accountService.DeleteAccount(await accountService.GetById(accountId));
                settingsManager.LastDatabaseUpdate = DateTime.Now;
                Close(this);
            }
            balanceViewModel.UpdateBalanceCommand.Execute();
        }

        private void UpdateList()
        {
            messenger.Publish(new PaymentListFilterChangedMessage(this, IsClearedFilterActive, IsRecurringFilterActive));
        }
    }
}
