using System;
using System.Threading.Tasks;
using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Represents the Actions for a view.
    ///     On Windows this is a normaly in the app bar. 
    ///     On Android for example in a floating action button.
    /// </summary>
    public class PaymentListViewActionViewModel : MvxViewModel, IPaymentListViewActionViewModel
    {
        private readonly IAccountService accountService;
        private readonly ISettingsManager settingsManager;
        private readonly IDialogService dialogService;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly IMvxNavigationService navigationService;
        private readonly int accountId;

        public PaymentListViewActionViewModel(IAccountService accountService,
                                              ISettingsManager settingsManager,
                                              IDialogService dialogService,
                                              IBalanceViewModel balanceViewModel,
                                              IMvxNavigationService navigationService,
                                              int accountId)
        {
            this.accountService = accountService;
            this.settingsManager = settingsManager;
            this.dialogService = dialogService;
            this.balanceViewModel = balanceViewModel;
            this.navigationService = navigationService;
            this.accountId = accountId;
        }

        #region Commands

        public MvxAsyncCommand GoToAddIncomeCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Income)));

        public MvxAsyncCommand GoToAddExpenseCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Expense)));

        public MvxAsyncCommand GoToAddTransferCommand =>
            new MvxAsyncCommand(async () => await navigationService
                                    .Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(
                                        new ModifyPaymentParameter(PaymentType.Transfer)));

        public MvxAsyncCommand DeleteAccountCommand => new MvxAsyncCommand(DeleteAccount);

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
    }
}
