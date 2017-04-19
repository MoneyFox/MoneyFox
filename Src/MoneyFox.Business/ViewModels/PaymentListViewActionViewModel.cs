using System;
using System.Linq;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Represents the Actions for a view.
    ///     On Windows this is a normaly in the app bar. 
    ///     On Android for example in a floating action button.
    /// </summary>
    public class PaymentListViewActionViewModel : BaseViewModel, IPaymentListViewActionViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IPaymentManager paymentManager;
        private readonly ISettingsManager settingsManager;
        private readonly IDialogService dialogService;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly int accountId;

        public PaymentListViewActionViewModel(IAccountRepository accountRepository,
            IPaymentManager paymentManager,
            ISettingsManager settingsManager, 
            IDialogService dialogService, 
            IBalanceViewModel balanceViewModel, 
            int accountId)
        {
            this.accountRepository = accountRepository;
            this.paymentManager = paymentManager;
            this.settingsManager = settingsManager;
            this.dialogService = dialogService;
            this.balanceViewModel = balanceViewModel;
            this.accountId = accountId;
        }

        public MvxCommand GoToAddIncomeCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Income}));

        public MvxCommand GoToAddExpenseCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Expense }));

        public MvxCommand GoToAddTransferCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Transfer }));

        public MvxCommand DeleteAccountCommand => new MvxCommand(DeleteAccount);

        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable => accountRepository.GetList().Count() > 1;

        /// <summary>
        ///     Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable => accountRepository.GetList().Any();

        /// <summary>
        ///     Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable => accountRepository.GetList().Any();

        private async void DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                var accountToDelete = accountRepository.FindById(accountId);

                paymentManager.DeleteAssociatedPaymentsFromDatabase(accountToDelete);

                if (accountRepository.Delete(accountToDelete))
                {
                    settingsManager.LastDatabaseUpdate = DateTime.Now;
                    Close(this);
                }
                else
                {
                    await
                        dialogService.ShowConfirmMessage(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
                }
            }
            balanceViewModel.UpdateBalanceCommand.Execute();
        }
    }
}
