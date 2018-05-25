using MoneyFox.Business.Parameters;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
{
    /// <inheritdoc />
    public class AccountListViewActionViewModel : BaseViewModel, IAccountListViewActionViewModel
    {
        private readonly IAccountService accountService;
        private readonly IMvxNavigationService navigationService;

        public AccountListViewActionViewModel(IAccountService accountService, IMvxNavigationService navigationService)
        {
            this.accountService = accountService;
            this.navigationService = navigationService;
        }
        
        /// <inheritdoc />
        public MvxAsyncCommand GoToAddAccountCommand =>
                new MvxAsyncCommand(async () => await navigationService.Navigate<ModifyAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter()));
        
        /// <inheritdoc />
        public MvxAsyncCommand GoToAddIncomeCommand =>
                new MvxAsyncCommand(async () => await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Income)));
        
        /// <inheritdoc />
        public MvxAsyncCommand GoToAddExpenseCommand =>
            new MvxAsyncCommand(async () => await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Expense)));

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddTransferCommand =>
            new MvxAsyncCommand(async () => await navigationService.Navigate<ModifyPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Transfer)));

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
    }
}
